<template>
  <div class="form-container">
    <h2 class="login-title">Login</h2>
      <UForm :state="user" class="space-y-4" @submit="submit">
        <UFormGroup label="User" name="user" class="custom-label">
          <UInput v-model="user.username" color="purple"
          variant="outline"/>
        </UFormGroup>

        <UFormGroup label="Password" name="password" class="custom-label">
          <UInput v-model="user.password" type="password" color="purple"
          variant="outline"/>
        </UFormGroup>

        <UButton type="submit" class="login-button">
          Login
        </UButton>
      </UForm>
  </div>
</template>

<script setup lang="ts">
import {ref} from 'vue';

const auth = useAuth();
const user: User = {username: '', password: ''};

let showError = ref(false);

const submit = async () => {
  console.log("User object:", user);
  const isAuthenticated = await auth.logIn(user);
  console.log("Is authenticated:", isAuthenticated);
  if (isAuthenticated) {
    await navigateTo('/players'); // Redirect after login
  }
};
</script>

<style scoped>
.form-container {
  background-color: #fbfdf0; 
  border-radius: 1rem; /* Ümarad nurgad */
  padding: 2rem; 
  box-shadow: 0 4px 12px #9f57e2; /* Kerge vari  SIIIIAAAAA*/
  width: 100%;
  max-width: 300px; /* Maksimaalne laius */
  display: flex;
  flex-direction: column;
  align-items: center;
}

.login-title {
  font-size: 2rem;
  font-weight: bold;
  margin-bottom: 1rem;
  text-align: center;
}

.login-button {
  background-color: #8529ba; 
  color: white; 
  border-radius: 0.5rem; 
  padding: 0.5rem 1rem;
  font-size: 1rem;
  border: none;
  cursor: pointer;
  transition: background-color 0.3s;
}

.login-button:hover {
  background-color: #1a1f5c; 
}

.custom-label label {
  color: black !important;
}

</style>