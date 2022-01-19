import 'axios';
import axios from 'axios';

export class KeyBundleDto {
    /**
     * 
     * @param {String} hostKey 
     * @param {String} leadKey 
     * @param {String} playerKey 
     */
    constructor(hostKey, leadKey, playerKey) {
        this.hostKey = hostKey;
        this.leadKey = leadKey;
        this.playerKey = playerKey;
    }
}
export class ScheduleDatesDto {
    constructor(obj) {
        this.start = new Date(obj.start);
        this.end = new Date(obj.end);
    }
}
export class PlayerDto {
    /**
     * @param {*} obj
     */
    constructor(obj) {
        /** @type {String} */
        this.name = obj.name;
        /** @type {ScheduleDatesDto[]} */
        this.schedule = obj.schedule && obj.schedule.map(d => new ScheduleDatesDto(d));
    }
}
export class SessionDto {
    /**
     * @param {String} playerKey
     * @param {String} hostKey 
     * @param {String} leadKey 
     * @param {String} status 
     * @param {String} title 
     * @param {String} description 
     * @param {*} host 
     * @param {*} lead 
     * @param {*} players 
     * @param {*} finalSchedule 
     */
    constructor(playerKey, hostKey, leadKey, status, title, description, host, lead, players, finalSchedule) {
        this.playerKey = playerKey;
        this.hostKey = hostKey;
        this.leadKey = leadKey;
        this.status = status;
        this.title = title;
        this.description = description;
        /** @type {PlayerDto} */
        this.host = host && new PlayerDto(host);
        /** @type {PlayerDto} */
        this.lead = lead && new PlayerDto(lead);
        /** @type {PlayerDto[]} */
        this.players = players && players.map(p => new PlayerDto(p));
        /** @type {ScheduleDatesDto[]} */
        this.finalSchedule = finalSchedule && finalSchedule.map(d => new ScheduleDatesDto(d));
    }
}

export default class Api {
    constructor() { }

    /**
     * GET: api/sessions/{key}
     * @param {String} key 
     * @returns {SessionDto}
     */
    async getSession(key) {
        try {
            let result = await axios.get('api/sessions/' + key);
            if (result.data) {
                let d = result.data;
                return new SessionDto(d.playerKey, d.hostKey, d.leadKey, d.status, d.title, d.description, d.host, d.lead, d.players, d.finalizedSchedule);
            }
            return null;
        } catch (error) {
            return null;
        }
    }

    /**
     * POST: api/sessions
     * @param {String} name 
     * @param {String} title 
     * @param {String} description 
     * @returns {KeyBundleDto}
     */
    async createSession(name, title, description) {
        try {
            let result = await axios.post('api/sessions', {
                name: name,
                title: title,
                description: description
            });
            if (result.data) {
                return new KeyBundleDto(result.data.hostKey, result.data.leadKey, result.data.playerKey);
            }
            return null;
        } catch (error) {
            return null;
        }
    }

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