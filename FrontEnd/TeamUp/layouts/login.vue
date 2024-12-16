<template>
  <div class="hero-container">
    <!-- Teksti osa -->
    <div class="text-section">
      <main class="title">Welcome to</main>
      <h1 class="title2">Team UP!</h1>
    </div>

    <!-- LoginForm osa -->
    <div class="form-section">
      <slot />
    </div>
  </div>
</template>

<style scoped>
.hero-container {
  display: flex; /* Asetab elemendid kõrvuti */
  justify-content: space-between; /* Lisab ruumi tekstile ja vormile */
  align-items: center; /* Vertikaalne joondamine */
  min-height: 100vh; /* Katab kogu kõrguse */
  padding: 8rem; 
  background-color: #cee7f8; /* Õrn sinine taust */
}

.text-section {
  flex: 1; /* Võtab rohkem ruumi vasakul */
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: flex-start;
}

.title {
  font-size: 4rem;
  font-weight: 600;
  color: #333;
  margin: 0;
}

.title2 {
  font-size: 6rem;
  font-family: "Anton", sans-serif;
  font-weight: 900;
  color: #202a79;
  margin: 0;
}

.form-section {
  flex: 0.5; /* Võtab vähem ruumi paremal */
  display: flex;
  justify-content: center;
}
</style>
