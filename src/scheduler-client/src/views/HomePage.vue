<template>
  <div class="container">
    <div class="row">
      <div class="col-12">
        <div>Home Page</div>
        <div class="col-6 col-lg-3">
          <create-component
            @create="handleCreate"
            :disabled="createFormDisabled"
            :error-message="createFormErrorMessage"
          />
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import CreateComponent, {
  CreateEventData,
} from "../components/CreateComponent.vue";
import Api from "../scripts/SessionApi";
const api = new Api();

export default {
  components: { CreateComponent },
  name: "Home",
  data() {
    return {
      createFormDisabled: false,
      createFormErrorMessage: '',
    };
  },
  methods: {
    /** @param {CreateEventData} createData */
    async handleCreate(createData) {
      this.createFormErrorMessage = '';
      this.createFormDisabled = true;
      
      if (createData.name && createData.title && createData.description) {
        let keys = await api.createSession(
          createData.name,
          createData.title,
          createData.description
        );
        if (keys) {
          this.$router.push({ path: `/${keys.leadKey}` });
        } else {
          this.createFormErrorMessage = 'failed to create keys';
        }
      }
      this.createFormDisabled = false;
    },
  },
};
</script>
