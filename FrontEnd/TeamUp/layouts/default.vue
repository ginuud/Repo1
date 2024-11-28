<template>
  <div class="layout">
    <header class="header flex items-center justify-between px-8 py-4" :class="{ 'header-small': isScrolled }">
      <TheMenu />
      <div class="flex-1"></div>
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

// scroll-listeneri, et tuvastada, kui kasutaja kerib
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
  color: #333; 
  min-height: 100vh; 
  display: flex;
  flex-direction: column;
}

.header {
  background: linear-gradient(90deg, #9f57e2, #cee7f8);
  color: white;
  height: 80px; 
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 0 20px; 
  position: sticky; /* Kinnitub ülaossa scrollimisel */
  top: 0;
  z-index: 1000; /* Et jääks alati nähtavaks */
  transition: all 0.3s ease; /* Sujuvad muutused */
}

.header-small {
  height: 50px; /* Väiksem kõrgus scrollimisel */
  padding: 0 15px;
  box-shadow: 0 2px 8px rgba(36, 124, 240, 0.2); 
}

.content {
  flex: 1; /* Täidab ülejäänud ruumi */
}

.footer {
  background-color: #cbdef7; 
  color: white;
}

.header .add-player-button:hover {
  transform: scale(1.05); 
  transition: transform 0.2s ease;
}
</style>


