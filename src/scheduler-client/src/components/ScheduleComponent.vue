<template>
  <div>
    <ul class="list-group">
      <li
        class="list-group-item list-group-item-primary"
        v-if="days.length && days[0].day != 1"
      >
        {{ days[0].month }}
      </li>
      <template v-for="day in days">
        <li
          class="list-group-item list-group-item-primary"
          :key="day.key + '-daypanel'"
          v-if="day.day == 1"
        >
          {{ day.month }}
        </li>
        <li
          class="list-group-item"
          :key="day.key"
          :class="{
            'list-group-item-secondary': ['Sun', 'Sat'].includes(day.name),
          }"
        >
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
                  'btn-primary': day.selected(hour) && !day.hourMap[hour],
                  'btn-success': day.selected(hour) && day.hourMap[hour],
                  'btn-outline-secondary':
                    !day.selected(hour) && day.selectable(hour) && !day.hourMap[hour],
                  'btn-outline-success':
                    !day.selected(hour) && day.selectable(hour) && day.hourMap[hour],
                  'btn-secondary': !day.selectable(hour),
                  
                }"
                @click="hourSelected(day, hour)"
              >
                {{ hour > 12 ? hour % 12 : hour }}
                <span
                  class="badge bg-success"
                  v-show="day.hourMap[hour]"
                  >{{ day.otherSelectedCount(hour) }}</span
                >
              </button>
            </div>
          </div>
        </li>
      </template>
    </ul>
  </div>
</template>

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
  constructor(date, startHour, endHour, selectableHours, othersSelectedHours) {
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

    this.hourMap = {}
    selectableHours.forEach(hour => this.hourMap[hour] = this.othersSelectedHours.filter((h) => h == hour).length);
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

  otherSelectedCount(hour) {
    return this.selected(hour) ? this.hourMap[hour] + 1 : this.hourMap[hour];
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
  props: {
    startDate: Date,
    endDate: Date,
    startHour: Number,
    endHour: Number,
    selectableHours: Array,
    selectedHours: Array,
  },
  data: () => ({
    /** @type {DayViewmodel[]} */
    days: [],
  }),
  mounted() {
    this.recalculateDays();
  },
  watch: {
    startDate() {
      this.recalculateDays();
    },
    endDate() {
      this.recalculateDays();
    },
    startHour() {
      this.recalculateDays();
    },
    endHour() {
      this.recalculateDays();
    },
    selectableHours() {
      this.recalculateDays();
    },
  },
  methods: {
    recalculateDays() {
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
      day.toggle(hour);

      let selections = [];
      this.days
        .filter((d) => d.selectedHours.length)
        .forEach((d) => {
          selections.push(
            new ScheduleDaySelections(d.date, d.selectedHours.sort())
          );
        });

      this.$emit("input", selections);
    },
  },
};
</script>

<style>
</style>