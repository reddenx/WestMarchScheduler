export class ScheduleDatesDto { }
export class SessionDto { 
    constructor(playerKey, hostKey, leadKey, status, hostDto, leadName, )
}

export default class Api {
    constructor() { }

    /**
     * GET: api/sessions/{key}
     * @param {String} key 
     * @returns {SessionDto}
     */
    async getSession(key) { }

    /**
     * POST: api/sessions
     * @param {String} name 
     * @param {String} title 
     * @param {String} description 
     * @returns {SessionDto}
     */
    async createSession(name, title, description) { }

    /**
     * PUT: api/session/{key}/approve
     * @param {String} key 
     * @param {String} name 
     * @param {ScheduleDatesDto[]} schedule 
     */
    async approveSession(key, name, schedule) { }

    /**
     * PUT: api/sessions/{key}/schedule
     * @param {String} key 
     * @param {ScheduleDatesDto[]} schedule 
     */
    async leadSchedule(key, schedule) { }

    /**
     * PUT: api/sessions/{key}/join
     * @param {String} key 
     * @param {String} name 
     * @param {ScheduleDatesDto[]} schedule 
     */
    async playerJoin(key, name, schedule) { }

    /**
     * PUT: api/sessions/{key}/finalize
     * @param {String} key 
     * @param {ScheduleDatesDto[]} schedule 
     */
    async hostFinalize(key, schedule) { }
}