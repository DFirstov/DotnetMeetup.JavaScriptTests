const axios = require('axios')
const uuid = require('uuid')
const {createDbClient} = require("../Testing/dbUtils")
const {waitForProducing} = require("../Testing/kafkaUtils")

describe('POST GravityAcceleration', () => {
    const dbClient = createDbClient()

    test('POST GravityAcceleration saves object to database', async () => {
        // Act
        const gaName = uuid.v4()
        const ga = 123
        const url = 'http://localhost:5204/GravityAcceleration'
        await axios.post(url, {
            name: gaName,
            value: ga
        })

        // Assert
        const gaFromDb = await dbClient
            .select('*')
            .from('GravityAcceleration')
            .where({Name: gaName})

        expect(gaFromDb[0].Value).toBe(ga)
    })
})

test('POST GravityAcceleration produces message to Kafka', async () => {
    // Act
    const gaName = uuid.v4();
    const ga = 234
    const gaFromKafka = await waitForProducing('ga-created', gaName, async () => {
        const url = 'http://localhost:5204/GravityAcceleration'
        await axios.post(url, {
            name: gaName,
            value: ga
        })
    })

    // Assert
    expect(parseFloat(gaFromKafka)).toBe(ga)
}, 10_000)
