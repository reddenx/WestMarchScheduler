<template>
  <div class="home">
    <table style="margin: auto; border-spacing: 0">
      <tr>
        <td>x</td>
        <td v-for="day in days" :key="day.date.toISOString()">
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
            weekend: day.isWeekend
          }"
          v-for="day in days"
          :key="day.date.toISOString()"
          style="border: 1px solid black; min-width: 3em; height: 2em"
          @mousedown.prevent="mousedown($event, day, hour)"
          @mouseup.prevent="mouseup($event, day, hour)"
          @mouseenter.prevent="mouseenter($event, day, hour)"
          @touchstart.prevent="touchstart($event, day, hour)"
          @touchmove.prevent="touchmove($event, day, hour)"
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
.weekend {
  background-color: rgb(224, 224, 224);
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
  constructor(date, startHour, endHour, selectableHours, othersSelectedHours) {
    /** @type {String} */
    this.name = days[date.getDay()];
    this.isWeekend = weekendDays.includes(this.name);
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
const weekendDays = ["Sun", "Sat"];
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
  props: {
    startDate: Date,
    endDate: Date,
    startHour: Number,
    endHour: Number,
    selectableHours: Array,
    selectedHours: Array,
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
  data: () => ({
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
      this.hours = [];
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
            new ScheduleDaySelections(d.date, d.selectedHours.sort())
          );
        });
      this.$emit("input", selections);
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
      this.hourSelected(day, hour);
    },
  },
};
</script>
