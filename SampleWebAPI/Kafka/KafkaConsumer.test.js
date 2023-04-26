const uuid = require("uuid")
const {waitForConsuming} = require("../Testing/kafkaUtils")
const {getReceivedRequestBodies} = require("../Testing/mockUtils")

test.skip('Message in Kafka leads to posting GA to the service', async () => {
    // Act
    const gaName = uuid.v4()
    const ga = 345
    await waitForConsuming('ga', 'ga-created', gaName, ga)
    
    // Assert
    const requestBodies = await getReceivedRequestBodies('/gravityAcceleration')
    expect(requestBodies).toContainEqual({
        name: gaName,
        value: ga
    })
}, 10_000)
