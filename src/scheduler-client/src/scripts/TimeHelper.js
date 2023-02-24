export class TimeSpan {
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

//does this need to be a class you dork!?
export class HourBlock {
    /**
     * 
     * @param {Date} hour 
     */
    constructor(hour) {
        this.hour = hour;
    }
}


/**
 * @param {TimeSpan[]} timespans 
 * @returns {HourBlock[]}
 */
export function timeSpansToHourBlocks(timespans) {
    let blocks = [];
    timespans.forEach(s => {
        for (let i = s.start.getHours(); i < s.end.getHours(); ++i) {
            blocks.push(new HourBlock(new Date(s.start.getFullYear(), s.start.getMonth(), s.start.getDate(), i)));
        }
    })
    return blocks;
}

/**
 * @param {HourBlock[]} hourblocks
 * @returns {TimeSpan[]} 
 */
export function hourBlocksToTimeSpans(hourblocks) {
    if (!hourblocks.length) return [];
    if (hourblocks.length == 1) {
        let hour = new Date(hourblocks[0].hour);
        let start = extractHour(hour);
        let end = addHours(start, 1);
        return [new TimeSpan(start, end)];
    }

    let sortedHours = hourblocks.map(h => h.hour).sort((a, b) => a > b ? 1 : -1);

    //grab next and set as start and current
    //grab next, if it's an hour ahead set as current and grab next, repeat until greater than an hour
    //once greater than an hour, create span from start to current, set grabbed next as start, and repeat the whole process
    let spans = [];
    let index = 0;
    let start = new Date(sortedHours[index]);
    let current = start;
    let next = new Date(sortedHours[++index]);
    while (next) {
        next = new Date(next);
        let diff = getHourDifference(next, current);
        if (diff > 1) {
            spans.push(new TimeSpan(start, addHours(current, 1)));
            start = next;
            current = next;
        }

        current = next;
        next = sortedHours[++index];
    }
    spans.push(new TimeSpan(start, addHours(current, 1)));

    return spans;
}


/**
 * @param {Date} a 
 * @param {Date} b 
 * @returns {Number}
 */
function getHourDifference(a, b) {
    return (a - b) / (1000 * 3600);
}


/** @param {Date} date */
function extractHour(date) {
    return new Date(date.getFullYear(), date.getMonth(), date.getDate(), date.getHours());
}

/**
 * @param {Date} date 
 * @param {Number} hours 
 */
function addHours(date, hours) {
    return new Date(date.getFullYear(), date.getMonth(), date.getDate(), date.getHours() + hours);
}