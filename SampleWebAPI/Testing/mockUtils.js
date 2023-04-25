const axios = require("axios")

async function configureGravityAccelerationMockOK(gaName, ga) {
    const mapping = {
        request: {
            method: 'GET',
            urlPattern: `/gravityAcceleration/${gaName}`
        },
        response: {
            jsonBody: {
                name: gaName,
                value: ga
            }
        }
    }

    await axios.post('http://localhost:5433/__admin/mappings', mapping)
}

async function configureGravityAccelerationMock503(gaName) {
    const mapping = {
        request: {
            method: 'GET',
            urlPattern: `/gravityAcceleration/${gaName}`
        },
        response: {
            status: 503
        }
    }

    await axios.post('http://localhost:5433/__admin/mappings', mapping)
}

module.exports = {
    configureGravityAccelerationMockOK,
    configureGravityAccelerationMock503
}
