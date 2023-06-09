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

async function getReceivedRequestBodies(url) {
    const response = await axios.get('http://localhost:5433/__admin/requests')
    return response.data.requests
        .filter(request => request.request.url === url)
        .map(request => JSON.parse(request.request.body.toString()))
}

module.exports = {
    configureGravityAccelerationMockOK,
    configureGravityAccelerationMock503,
    getReceivedRequestBodies
}
