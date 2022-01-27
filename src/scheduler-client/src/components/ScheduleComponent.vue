<template>
    <div>
        <ul class="list-group">
            <li class="list-group-item" v-for="day in days" :key="day.date">
                <div class="row">
                    <div class="col-auto">
                        {{ day.name }} {{ day.day }}{{ suffix(day.day) }}
                    </div>
                    <div class="col">
                        <button
                            class="btn"
                            v-for="hour in day.hours"
                            :key="hour"
                            :disabled="!day.selectable(hour)"
                            :class="{
                                'btn-primary': day.selected(hour),
                                'btn-outline-secondary':
                                    !day.selected(hour) && day.selectable(hour),
                                'btn-secondary': !day.selectable(hour),
                            }"
                            @click="day.toggle(hour)"
                        >
                            {{ hour > 12 ? hour % 12 : hour }}
                        </button>
                    </div>
                </div>
            </li>
        </ul>
    </div>
</template>

<script>
class DayViewmodel {
    /** @param {Date} date */
    constructor(date, startHour, endHour, selectableHours) {
        /** @type {String} */
        this.name = days[date.getDay()];
        //this.month = months[date.getMonth()];
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

        this.selectedHours = [];
    }

    toggle(hour) {
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
}

const days = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];

export default {
    props: {
        startDate: Date,
        endDate: Date,
        startHour: Number,
        endHour: Number,
        selectableHours: Array,
    },
    data: () => ({
        days: [],
    }),
    mounted() {
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
        while (start < this.endDate) {
            this.days.push(
                new DayViewmodel(
                    start,
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
                            .map((h) => h.hour)
                )
            );
            start.setDate(start.getDate() + 1);
        }
    },
    methods: {
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
    },
};
</script>

<style>
</style>