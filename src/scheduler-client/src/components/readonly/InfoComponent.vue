<template>
    <div class="card">
        <div class="card-header">
            <h1>{{ session.title }}</h1>
        </div>
        <div class="card-body">
            <div class="expand-container">
                <p v-if="!canExpandDescription" class="description">
                    {{ session.description }}
                </p>
                <p v-if="canExpandDescription" class="description">
                    {{ descriptionExpanded ? session.description : shortenedDescription }}
                    <a v-show="!descriptionExpanded" class="float-end" @click="toggleExpand"> Read More </a>
                </p>

            </div>
        </div>
    </div>
</template>

<script>
import { SessionViewmodel } from "../../scripts/CommonModels";

const DESC_SHORTEN_LEN = 60;

export default {
    props: {
        session: SessionViewmodel,
    },
    data: () => ({
        descriptionExpanded: false,
        canExpandDescription: false,
        shortenedDescription: "",
    }),
    mounted() {
        if (this.session.description.length > DESC_SHORTEN_LEN) {
            this.shortenedDescription =
                this.session.description.substring(0, DESC_SHORTEN_LEN) + "...";
            this.canExpandDescription = true;
        }
    },
    methods: {
        toggleExpand() {
            if (!this.canExpandDescription) return;
            this.descriptionExpanded = !this.descriptionExpanded;
        },
    },
};
</script>

<style>
h1 {
    font-size: 2em;
    text-transform: capitalize;
}

.description {
    text-align: left;
}
</style>