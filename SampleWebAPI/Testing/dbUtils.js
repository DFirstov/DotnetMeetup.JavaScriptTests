const knex = require("knex")

function createDbClient() {
    const dbClient = knex({
        client: 'pg',
        connection: {
            host: 'localhost',
            port: '5432',
            user: 'postgres',
            password: 'js-tests'
        }
    })

    afterAll(async () => {
        await dbClient.destroy()
    })

    return dbClient
}

module.exports = {
    createDbClient
}
