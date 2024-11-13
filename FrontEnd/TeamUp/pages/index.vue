<template>
    <div class="hero">
      <h1 class="title">Team UP</h1>
      <button class="start-button" @click="goToStart">Get Started</button>
    </div>
  </template>
  
  <script setup>
  const goToStart = () => {
    // Kasutame Nuxt Routerit, et suunata kasutaja teisele lehele
    useRouter().push('/players');
  };
  </script>
  
  <style scoped>
  .hero {
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    height: 100vh;
    text-align: center;
    background-color: #e8e8d9;
  }
  
  .title {
    font-size: 4rem;
    font-weight: bold;
    color: #333;
    margin-bottom: 1rem;
  }
  
  .start-button {
    padding: 0.75rem 1.5rem;
    font-size: 1.25rem;
    background-color: #007bff;
    color: #fff;
    border: none;
    border-radius: 5px;
    cursor: pointer;
    transition: background-color 0.3s;
  }
  
  .start-button:hover {
    background-color: #0056b3;
  }
  </style>
  
  