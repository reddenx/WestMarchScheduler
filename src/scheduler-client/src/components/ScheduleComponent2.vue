<template>
    <div class="home">
        <table style="margin: auto; border-spacing: 0">
            <tr>
                <td>x</td>
                <td v-for="day in days" :key="day.name">
                    {{ day.day }}{{ suffix(day.day) }}
                </td>
            </tr>

            <tr v-for="hour in hours" :key="hour">
                <td>{{ hour }}</td>
                <td
                    class="selectbox"
                    :class="{
                        selectable: day.selectable(hour),
                        unselectable: !day.selectable(hour),
                        otherSelected: day.otherSelectedCount(hour) > 0,
                        selected: day.selected(hour),
                    }"
                    v-for="day in days"
                    :key="day.name"
                    style="border: 1px solid black; min-width: 3em; height: 2em"
                    @mousedown.prevent="mousedown($event, day, hour)"
                    @mouseup.prevent="mouseup($event, day, hour)"
                    @mouseenter.prevent="mouseenter($event, day, hour)"
                    @touchstart.prevent="touchstart($event, day, hour)"
                >
                    <span v-show="day.otherSelectedCount(hour) > 0">{{
                        day.otherSelectedCount(hour)
                    }}</span>
                </td>
            </tr>
        </table>
    </div>
</template>

<style scoped>
.selectable {
    background-color: white;
    cursor: pointer;
}
.selectable:hover {
    background-color: rgb(190, 216, 255);
}

.otherSelected {
    background-color: rgb(193, 255, 181);
}

.selected {
    background-color: rgb(91, 135, 230);
}

.unselectable {
    background-color: #cccccc;
}
</style>

<script>
export class ScheduleDaySelections {
    /**
     * @param {Date} date
     * @param {Number[]} hours
     */
    constructor(date, hours) {
        this.date = date;
        this.hours = hours;
    }
}

class DayViewmodel {
    /**
     * @param {Date} date
     * @param {Number} startHour
     * @param {Number} endHour
     * @param {Number[]} selectableHours
     * @param {Number[]} othersSelectedHours
     */
    constructor(
        date,
        startHour,
        endHour,
        selectableHours,
        othersSelectedHours
    ) {
        /** @type {String} */
        this.name = days[date.getDay()];
        this.month = months[date.getMonth()];
        this.day = date.getDate();
        /** @type {Number} */
        this.startHour = startHour;
        /** @type {Number} */
        this.endHour = endHour;
        /** @type {Number[]} */
        this.hours = [];
        for (let i = this.startHour; i < this.endHour; ++i) {
            this.hours.push(i);
        }
        /** @type {Number[]} */
        this.selectableHours = selectableHours || this.hours;
        this.othersSelectedHours = othersSelectedHours || [];
        this.selectedHours = [];
        this.key = date.toUTCString();
        this.date = date;
        this.hourMap = {};
        selectableHours.forEach(
            (hour) =>
                (this.hourMap[hour] = this.othersSelectedHours.filter(
                    (h) => h == hour
                ).length)
        );
    }
    toggle(hour) {
        if (!this.selectable(hour)) return;
        let index = this.selectedHours.indexOf(hour);
        if (index >= 0) {
            this.selectedHours.splice(index, 1);
        } else {
            this.selectedHours.push(hour);
        }
    }
    selectable(hour) {
        return this.selectableHours.includes(hour);
    }
    selected(hour) {
        return this.selectedHours.includes(hour);
    }
    otherSelectedCount(hour) {
        return this.hourMap[hour];
        // return this.selected(hour)
        //     ? this.hourMap[hour] + 1
        //     : this.hourMap[hour];
    }
}
const days = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];
const months = [
    "January",
    "February",
    "March",
    "April",
    "May",
    "June",
    "July",
    "August",
    "September",
    "October",
    "November",
    "December",
];

export default {
    components: {},
    data: () => ({
        //inputs
        startDate: new Date(2023, 2, 2),
        endDate: new Date(2023, 2, 20),
        startHour: 8,
        endHour: 22,
        selectableHours: [
            new Date(2023, 2, 5, 10),
            new Date(2023, 2, 5, 11),
            new Date(2023, 2, 5, 12),
            new Date(2023, 2, 5, 13),
            new Date(2023, 2, 5, 14),
            new Date(2023, 2, 5, 15),
            new Date(2023, 2, 5, 16),
            new Date(2023, 2, 5, 17),
            new Date(2023, 2, 5, 18),
            new Date(2023, 2, 5, 19),
            new Date(2023, 2, 5, 20),

            new Date(2023, 2, 6, 10),
            new Date(2023, 2, 6, 11),
            new Date(2023, 2, 6, 12),
            new Date(2023, 2, 6, 13),
            new Date(2023, 2, 6, 14),
            new Date(2023, 2, 6, 15),
            new Date(2023, 2, 6, 16),
            new Date(2023, 2, 6, 17),
            new Date(2023, 2, 6, 18),
            new Date(2023, 2, 6, 19),
            new Date(2023, 2, 6, 20),

            new Date(2023, 2, 7, 10),
            new Date(2023, 2, 7, 11),
            new Date(2023, 2, 7, 12),
            new Date(2023, 2, 7, 13),
            new Date(2023, 2, 7, 14),
            new Date(2023, 2, 7, 15),
            new Date(2023, 2, 7, 16),
            new Date(2023, 2, 7, 17),
            new Date(2023, 2, 7, 18),
            new Date(2023, 2, 7, 19),
            new Date(2023, 2, 7, 20),

            new Date(2023, 2, 8, 10),
            new Date(2023, 2, 8, 11),
            new Date(2023, 2, 8, 12),
            new Date(2023, 2, 8, 13),
            new Date(2023, 2, 8, 14),
            new Date(2023, 2, 8, 15),
            new Date(2023, 2, 8, 16),
            new Date(2023, 2, 8, 17),
            new Date(2023, 2, 8, 18),
            new Date(2023, 2, 8, 19),
            new Date(2023, 2, 8, 20),

            new Date(2023, 2, 9, 10),
            new Date(2023, 2, 9, 11),
            new Date(2023, 2, 9, 12),
            new Date(2023, 2, 9, 13),
            new Date(2023, 2, 9, 14),
            new Date(2023, 2, 9, 15),
            new Date(2023, 2, 9, 16),
            new Date(2023, 2, 9, 17),
            new Date(2023, 2, 9, 18),
            new Date(2023, 2, 9, 19),
            new Date(2023, 2, 9, 20),
        ], //list of Date objects
        selectedHours: [
            new Date(2023, 2, 9, 14),
            new Date(2023, 2, 9, 15),
            new Date(2023, 2, 9, 16),
            new Date(2023, 2, 9, 14),
            new Date(2023, 2, 9, 15),
            new Date(2023, 2, 9, 16),
            new Date(2023, 2, 8, 13),
            new Date(2023, 2, 8, 14),
            new Date(2023, 2, 8, 15),
            new Date(2023, 2, 8, 16),
            new Date(2023, 2, 8, 17),
        ], //list of Date objects

        //internal bindings
        weeks: [],
        hours: [],
        days: [],
        erasing: false,
    }),
    mounted() {
        this.recalculateDays();
    },
    methods: {
        recalculateDays() {
            for (let i = this.startHour; i <= this.endHour; i++) {
                this.hours.push(i);
            }

            /** @type {DayViewmodel} */
            this.days = [];
            if (this.startDate > this.endDate) {
                return;
            }
            let start = new Date(this.startDate);
            let selectableHours =
                this.selectableHours &&
                this.selectableHours.map((h) => ({
                    year: h.getYear(),
                    month: h.getMonth(),
                    day: h.getDate(),
                    hour: h.getHours(),
                }));
            let selectedHours =
                this.selectedHours &&
                this.selectedHours.map((h) => ({
                    year: h.getYear(),
                    month: h.getMonth(),
                    day: h.getDate(),
                    hour: h.getHours(),
                }));
            while (start < this.endDate) {
                this.days.push(
                    new DayViewmodel(
                        new Date(start),
                        this.startHour,
                        this.endHour,
                        selectableHours &&
                            selectableHours
                                .filter(
                                    (h) =>
                                        h.year == start.getYear() &&
                                        h.month == start.getMonth() &&
                                        h.day == start.getDate()
                                )
                                .map((h) => h.hour),
                        selectedHours &&
                            selectedHours
                                .filter(
                                    (h) =>
                                        h.year == start.getYear() &&
                                        h.month == start.getMonth() &&
                                        h.day == start.getDate()
                                )
                                .map((h) => h.hour)
                    )
                );
                start.setDate(start.getDate() + 1);
            }
        },
        suffix(day) {
            switch (day % 10) {
                case 1:
                    return "st";
                case 2:
                    return "nd";
                case 3:
                    return "rd";
                default:
                    return "th";
            }
        },
        hourSelected(day, hour) {
            if (day.selected(hour) != this.erasing) return;
            day.toggle(hour);
            let selections = [];
            this.days
                .filter((d) => d.selectedHours.length)
                .forEach((d) => {
                    selections.push(
                        new ScheduleDaySelections(
                            d.date,
                            d.selectedHours.sort()
                        )
                    );
                });
            this.$emit("input", selections);
            console.log(selections);
        },
        mousedown(event, day, hour) {
            this.erasing = day.selected(hour);
            if (event.buttons == 1) this.hourSelected(day, hour);
        },
        mouseup(event, day, hour) {
            if (event.buttons == 1) this.hourSelected(day, hour);
        },
        mouseenter(event, day, hour) {
            if (event.buttons == 1) this.hourSelected(day, hour);
        },
        touchstart(event, day, hour) {
            this.erasing = day.selected(hour);
        },
    },
};
</script>
