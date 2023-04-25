const kafkajs = require("kafkajs")

async function waitForMessage(topic, key, action) {
    const kafka = new kafkajs.Kafka({brokers: ['127.0.0.1:9093']})
    const consumer = kafka.consumer({groupId: 'js-tests'})
    await consumer.connect()
    await consumer.subscribe({topic: topic})

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

module.exports = {
    waitForMessage
}
