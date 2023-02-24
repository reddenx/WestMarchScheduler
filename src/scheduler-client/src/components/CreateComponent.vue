<template>
  <div class="card">
    <div class="card-body text-start">
      <div class="row">
        <div class="col-12">
          <form>
            <div class="mb-3">
              <label class="form-label">Your Name</label>
              <input
                type="text"
                class="form-control"
                v-model="nameInput"
                :disabled="disabled"
              />
            </div>
            <div class="mb-3">
              <label class="form-label">Title</label>
              <input
                type="text"
                class="form-control"
                v-model="titleInput"
                :disabled="disabled"
              />
            </div>
            <div class="mb-3">
              <label class="form-label">Description</label>
              <input
                type="text"
                class="form-control"
                v-model="descriptionInput"
                :disabled="disabled"
              />
            </div>
            <div class="text-end">
              <button
                type="button"
                class="btn btn-primary"
                @click="createButtonPressed"
                :disabled="disabled || !validInput"
              >
                Create
              </button>
            </div>
            <div class="mt-3 alert alert-danger" v-show="errorMessage">There was an error creating your event</div>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export class CreateEventData {
  /**
   * @param {String} name
   * @param {String} title
   * @param {String} description
   */
  constructor(name, title, description) {
    this.name = name;
    this.title = title;
    this.description = description;
  }
}

export default {
  props: {
    disabled: Boolean,
    errorMessage: String,
  },
  data() {
    return {
      titleInput: "",
      descriptionInput: "",
      nameInput: "",
    };
  },
  computed: {
    validInput() {
      return this.titleInput && this.descriptionInput && this.nameInput;
    },
  },
  methods: {
    createButtonPressed() {
      this.$emit(
        "create",
        new CreateEventData(
          this.nameInput,
          this.titleInput,
          this.descriptionInput
        )
      );
    },
  },
};
</script>

<style>
</style>