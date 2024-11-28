<!-- <template>
  <div>
    <div class="flex items-center justify-between px-8 py-2">
      <TheMenu />
      <div class="flex-1"></div>
      <div><AddPlayer class="items-center" /></div>
    </div>
    <div class="mt-4">
      <slot />
    </div>
  </div>
</template> -->
<template>
  <div class="layout">
    <header class="header flex items-center justify-between px-8 py-4" :class="{ 'header-small': isScrolled }">
      <TheMenu />
      <div class="flex-1"></div>
      <div>
        <AddPlayer class="add-player-button" />
      </div>
    </header>

    <main class="content mt-4">
      <slot />
    </main>

    <footer class="footer text-center py-4">
    </footer>
  </div>
</template>

<script setup>
import { ref, onMounted, onUnmounted } from "vue";

const isScrolled = ref(false);

// Lisame scroll-listeneri, et tuvastada, kui kasutaja kerib
const handleScroll = () => {
  isScrolled.value = window.scrollY > 50; // Muudame päise väiksemaks, kui kerime 50px
};

onMounted(() => {
  window.addEventListener("scroll", handleScroll);
});

onUnmounted(() => {
  window.removeEventListener("scroll", handleScroll);
});
</script>

<style scoped>
.layout {
  background-color: #cee7f8; 
  color: #333; /* Teksti värv */
  min-height: 100vh; /* Veebileht täidab kogu ekraani kõrguse */
  display: flex;
  flex-direction: column;
}

.header {
  background: linear-gradient(90deg, #9f57e2, #cee7f8);
  color: white;
  height: 80px; /* Esialgne kõrgus */
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 0 20px; /* Sisu marginaalid */
  position: sticky; /* Kinnitub ülaossa scrollimisel */
  top: 0;
  z-index: 1000; /* Et jääks alati nähtavaks */
  transition: all 0.3s ease; /* Sujuvad muutused */
}

.header-small {
  height: 50px; /* Väiksem kõrgus scrollimisel */
  padding: 0 15px; /* Väiksemad marginaalid */
  box-shadow: 0 2px 8px rgba(36, 124, 240, 0.2); /* Pehmem vari */
}

.content {
  flex: 1; /* Täidab ülejäänud ruumi */
}

.footer {
  background-color: #cbdef7; /* Jaluse taust tumesiniseks */
  color: white;
}

/* Hover-efekt menüüs */
.header .add-player-button:hover {
  transform: scale(1.05); /* Suurendab kergelt */
  transition: transform 0.2s ease;
}
</style>


