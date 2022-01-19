import { SessionDto, PlayerDto, ScheduleDatesDto } from './SessionApi'

export class ScheduleViewmodel {
    /**
     * 
     * @param {ScheduleDatesDto[]} dto 
     */
    constructor(dto) {
    }
}

export class PlayerViewmodel {
    /**
     * 
     * @param {String} type lead, host, or player
     * @param {PlayerDto} dto 
     */
    constructor(type, dto) {
        this.type = type;
        this.name = dto.name;
        this.schedule = new ScheduleViewmodel(dto.schedule);

        this.dto = dto;
    }
}

export class SessionViewmodel {
    /**
     * 
     * @param {SessionDto} dto 
     * @param {String} lookupKey
     */
    constructor(dto, lookupKey) {
        this.lookupKey = lookupKey;
        this.leadKey = dto.leadKey;
        this.hostKey = dto.hostKey;
        this.playerKey = dto.playerKey;
        /** @type {String} lead, host, player, or unknown */
        this.lookupType = 'unknown';
        if (lookupKey == dto.leadKey) {
            this.lookupType = 'lead';
        } else if (lookupKey == dto.hostKey) {
            this.lookupType = 'host';
        } else if (lookupKey == dto.playerKey) {
            this.lookupType = 'player';
        }

        this.status = dto.status;
        this.title = dto.title;
        this.description = dto.description;
        this.host = dto.host && new PlayerViewmodel(dto.host);
        this.lead = dto.lead && new PlayerViewmodel(dto.lead);
        this.players = dto.players && dto.players.map(p => new PlayerViewmodel(p));
        this.finalSchedule = new ScheduleViewmodel(dto.finalSchedule);

        this.dto = dto;
    }
}