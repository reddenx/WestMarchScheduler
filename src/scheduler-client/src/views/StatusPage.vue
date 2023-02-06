<template>
  <div class="container mt-3">
    <div class="row">
      <div class="col-12" v-if="session">
        {{ session.status }}
        <status-component :session="session" @reload="handleReload" />
      </div>
    </div>
  </div>
</template>

<script>
import Api from "../scripts/SessionApi";
import {
  SessionViewmodel,
  PlayerViewmodel,
  ScheduleViewmodel,
} from "../scripts/CommonModels";
import StatusComponent from "../components/StatusComponent.vue";

const api = new Api();

export default {
  components: { StatusComponent },

  props: {
    lookupKey: String,
  },
  watch: {
    lookupKey() {
      this.loadData();
    },
  },
  data() {
    return {
      /** @type {SessionViewmodel} */
      session: null,
    };
  },
  async mounted() {
    await this.loadData();
  },
  methods: {
    async loadData() {
      let dto = await api.getSession(this.lookupKey);
      this.session = new SessionViewmodel(dto, this.lookupKey);
      document.title = `${this.session.title} (${this.session.lookupType})`;
    },
    async handleReload() {
      await this.loadData();
    }
  },
};
</script>

<style>
</style>