<template>
  <div class="hero">
    <div class="titles">
      <h1 class="title">Welcome to</h1>
      <h1 class="title2">Team UP!</h1>
    </div>
    <LoginForm />
  </div>
</template>

<style scoped>
.hero {
  display: flex;
  flex-direction: row;
  justify-content: center;
  align-items: center;
  height: 100vh;
  text-align: center;
  background-color: #e8e8d9;
  gap: 15rem;
}

.titles {
  display: flex;
  flex-direction: column; /* "Welcome to" ja "Team UP!" jäävad üksteise alla */
  align-items: center;
}

.title {
  font-size: 3rem;
  font-weight: medium;
  color: #333;
  margin-bottom: 0.1rem;
}
.title2 {
  font-size: 8rem;
  font-family: 'Anton', sans-serif;
  font-weight: 800;
  color: #202a79;
  margin-bottom: 1rem;
}
</style>

<!-- .start-button {
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
} -->