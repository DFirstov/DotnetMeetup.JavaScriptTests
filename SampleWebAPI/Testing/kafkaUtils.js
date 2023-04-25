const kafkajs = require("kafkajs")

async function waitForProducing(topic, key, action) {
    const kafka = createKafka()
    const consumer = await createConsumer(kafka, topic)

    const messages = []
    await consumer.run({
        eachMessage: async ({message}) => {
            messages.push(message)
        }
    })
    
    await action()
    
    while (true) {
        const message = messages.find(m => m.key.toString() === key)
        if (message) return message.value.toString()
        await new Promise(resolve => setTimeout(resolve, 100))
    }
}

async function waitForConsuming(groupId, topic, key, value) {
    const kafka = createKafka()
    
    const producer = await createProducer(kafka)
    const offset = await produceMessage(producer, topic, key, value)
    
    await waitOffsetConsuming(kafka, groupId, topic, offset)
}

function createKafka() {
    return new kafkajs.Kafka({
        brokers: ['127.0.0.1:9093']
    })
}

async function createConsumer(kafka, topic) {
    const consumer = kafka.consumer({groupId: 'js-tests'})
    await consumer.connect()
    await consumer.subscribe({topic: topic})
    return consumer
}

async function createProducer(kafka) {
    const producer = kafka.producer()
    await producer.connect()
    return producer
}

async function produceMessage(producer, topic, key, value) {
    const recordMetadata = await producer.send({
        topic,
        messages: [{
            key,
            value: value.toString()
        }]
    })
    
    return recordMetadata[0].baseOffset
}

async function waitOffsetConsuming(kafka, groupId, topic, offset) {
    const kafkaAdmin = kafka.admin()
    
    while (true) {
        const groupOffsets = await kafkaAdmin.fetchOffsets({
            groupId,
            topics: [topic]
        })
        
        const currentOffset = groupOffsets[0].partitions[0].offset
        if (parseInt(currentOffset) > parseInt(offset)) break
        await new Promise(resolve => setTimeout(resolve, 100))
    }
}

module.exports = {
    waitForProducing,
    waitForConsuming
}
