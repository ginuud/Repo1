<template>
  <div class="bg-gray-800 text-white">
    <div class="flex items-center justify-between py-4 px-6">
      <img src="@/components/teamup.png" alt="TeamUp logo" class="w-24 h-auto mr-4" />

      <nav class="flex space-x-4">
        <router-link
          v-for="link in links"
          :key="link.label"
          :to="link.to"
          class="text-lg px-4 py-2 rounded"
        >
          {{ link.label }}
        </router-link>
      </nav>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from "vue";
import { useRoute } from "vue-router";

const route = useRoute();

// Linkide andmed
const links = ref([
  {
    label: "Players",
    to: "/players",
  },
  {
    label: "Teams",
    to: "/teams",
  },
  {
    label: "Games",
    to: "/games",
  },
]);
</script>

<style scoped>

.bg-gray-800 {
  background-color: transparent; /* Läbipaistmatu tume taust */
  z-index: 1000; /* Veendu, et see oleks teistest kihtidest kõrgem */
  position: relative; /* Tagab z-index'i korrektse toimimise */
}

.router-link-active {
  border: 2px solid #070f8a;
  border-radius: 6px;
}

nav a {
  text-transform: uppercase;
  font-weight: bold;
}

nav a:hover {
  background-color: #070f8a; 
}
</style>

