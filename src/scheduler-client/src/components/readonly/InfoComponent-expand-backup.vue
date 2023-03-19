<template>
    <div class="container">
        <h1>{{ session.title }}</h1>
        <div class="expand-container">
            <p class="expanding-description expanded collapsed">
                {{ session.description }}
            </p>
            <div class="expand-separator">
                <button class="expand-button">
                    <span class="fa-solid fa-chevron-down"></span>
                </button>
            </div>
        </div>
    </div>
</template>

<script>
import { SessionViewmodel } from "../../scripts/CommonModels";
export default {
    props: {
        session: SessionViewmodel,
    },
};
</script>

<style>
h1 {
    font-size: 2em;
    text-transform: capitalize;
}

.expand-separator {
    border-top: 1px solid black;
    position: relative;
}
.expand-button {
    width: 2em;
    height: 2em;
    border: 1px solid black;
    background-color: white;
    border-radius: 1em;
    /* margin-top:0sem; */
    position: absolute;
    margin: auth;
    top: -1em;
}

.expanding-description {
    overflow: hidden;
    max-height: 2.5em;
}
.expanding-description.collapsed {
    /* background:  */
    /* font-size: 72px; */
    background: linear-gradient(
        180deg,
        rgba(0, 0, 0, 1) 0%,
        rgba(0, 0, 0, 0) 100%
    );
    -webkit-background-clip: text;
    -webkit-text-fill-color: transparent;
}
</style>