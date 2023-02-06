import { SessionDto, PlayerDto, ScheduleDatesDto } from './SessionApi'

export class TimeSegment {
    /**
     * 
     * @param {Date} start 
     * @param {Date} end 
     */
    constructor(start, end) {
        this.start = start;
        this.end = end;
    }
}

export class ScheduleViewmodel {
    /**
     * 
     * @param {ScheduleDatesDto[]} dto 
     */
    constructor(dto) {
        /** @type {TimeSegment[]}*/
        this.dates = dto.map(d => new TimeSegment(d.start, d.end));
    }
}

export class PlayerViewmodel {
    /**
     * 
     * @param {PlayerDto} dto 
     */
    constructor(dto) {
        this.name = dto.name;
        /** @type {ScheduleViewmodel} */
        this.schedule = null;
        if (dto.schedule) {
            this.schedule = new ScheduleViewmodel(dto.schedule);
        }

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

        this.status = dto.status;
        this.title = dto.title;
        this.description = dto.description;
        this.host = dto.host && new PlayerViewmodel(dto.host);
        this.lead = dto.lead && new PlayerViewmodel(dto.lead);
        this.players = dto.players && dto.players.map(p => new PlayerViewmodel(p));
        /** @type {ScheduleViewmodel} */        
        this.finalSchedule = null;
        dto.finalSchedule && (this.finalSchedule = new ScheduleViewmodel(dto.finalSchedule));


        if (lookupKey == dto.leadKey) {
            this.lookupType = 'lead';
        } else if (lookupKey == dto.hostKey) {
            this.lookupType = 'host';
        } else if (lookupKey == dto.playerKey) {
            this.lookupType = 'player';
        }
        this.dto = dto;
    }
}