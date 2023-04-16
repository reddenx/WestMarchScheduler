<template>
    <div class="container">
        <div class="row">
            <div v-if="session.leadKey" class="col-12 col-lg lead-link">
                <button
                    v-show="!leadLinkRevealed"
                    class="scheduler-link btn btn-lg btn-danger"
                    @click="leadLinkRevealed = true"
                >
                    <span class="fa-regular fa-clone"></span>
                    Your Link
                </button>
                <div v-show="leadLinkRevealed" class="input-group">
                    <button class="btn btn-lg btn-danger" type="button">
                        <span class="fa-regular fa-clone"></span>
                    </button>
                    <input
                        type="text"
                        class="form-control bg-danger-disabled"
                        disabled
                        :value="fullLeadLink"
                    />
                </div>
            </div>
            <div v-if="session.hostKey" class="col-12 col-lg host-link">
                <button
                    v-show="!hostLinkRevealed"
                    class="scheduler-link btn btn-lg btn-warning"
                    @click="hostLinkRevealed = true"
                >
                    <span class="fa-regular fa-clone"></span>
                    <span v-if="session.host">
                        Host Link For "{{ session.host.name }}"</span
                    >
                    <span v-if="!session.host && !session.leadKey">
                        Your Link</span
                    >
                    <span v-if="!session.host && session.leadKey">
                        Host Link</span
                    >
                </button>
                <div v-show="hostLinkRevealed" class="input-group">
                    <button class="btn btn-lg btn-warning" type="button">
                        <span class="fa-regular fa-clone"></span>
                    </button>
                    <input
                        type="text"
                        class="form-control bg-warning-disabled"
                        disabled
                        :value="fullHostLink"
                    />
                </div>
            </div>
            <div class="col-12 col-lg">
                <button
                    v-show="!playerLinkRevealed"
                    class="scheduler-link btn btn-lg btn-success"
                    @click="playerLinkRevealed = true"
                >
                    <span class="fa-regular fa-clone"></span>
                    Share With Players
                </button>
                <div v-show="playerLinkRevealed" class="input-group">
                    <button class="btn btn-lg btn-success" type="button">
                        <span class="fa-regular fa-clone"></span>
                    </button>
                    <input
                        type="text"
                        class="form-control bg-success-disabled"
                        disabled
                        :value="fullPlayerLink"
                    />
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import { SessionViewmodel } from "../../scripts/CommonModels";

export default {
    components: {},
    props: {
        session: SessionViewmodel,
    },
    data: () => ({
        leadLinkRevealed: false,
        fullLeadLink: "",
        hostLinkRevealed: false,
        fullHostLink: "",
        playerLinkRevealed: false,
        fullPlayerLink: "",
    }),
    mounted() {
        this.fullLeadLink = `${window.origin}/#/${this.session.leadKey}`;
        this.fullHostLink = `${window.origin}/#/${this.session.hostKey}`;
        this.fullPlayerLink = `${window.origin}/#/${this.session.playerKey}`;
    },
    methods: {},
};
</script>

<style>
.scheduler-link {
    width: 100%;
}
/* #dc3545 */
.form-control.bg-danger-disabled {
    background-color: #ed8b94;
    border-color: #ed8b94;
}
/* #ffc107 */
.form-control.bg-warning-disabled {
    background-color: #edd669;
    border-color: #edd669;
}
/* #198754 */
.form-control.bg-success-disabled {
    background-color: #4ba57b;
    border-color: #4ba57b;
}
</style>