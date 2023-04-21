const kafkajs = require('kafkajs');
const uuid = require("uuid");
const axios = require("axios");

test('Message in Kafka leads to posting GA to the service', async () => {
    const kafka = new kafkajs.Kafka({
        brokers: ['127.0.0.1:9093']
    });
    
    const producer = kafka.producer();
    await producer.connect();
    
    const message = {
        key: uuid.v4(),
        value: '987'
    };
    
    const recordMetadata = await producer.send({
        topic: 'ga-created',
        messages: [message]
    });
    
    const kafkaAdmin = kafka.admin();
    
    while (true) {
        const groupOffsets = await kafkaAdmin.fetchOffsets({
            groupId: 'ga',
            topics: ['ga-created']
        });
        
        const currentOffset = groupOffsets[0].partitions[0].offset;
        if (parseInt(currentOffset) > parseInt(recordMetadata[0].baseOffset))
            break;
        
        await new Promise(resolve => setTimeout(resolve, 100));
    }
    
    await kafkaAdmin.disconnect();
    
    const response = await axios.get('http://localhost:5433/__admin/requests');
    const allRequests = response.data.requests;

    const actualRequestBody = allRequests
        .filter(request => request.request.url === '/gravityAcceleration')
        .map(request => JSON.parse(request.request.body.toString()))
        .find(body => body.name === message.key);
    
    expect(actualRequestBody.value).toBe(987);
    
    await producer.disconnect();
}, 60_000);
