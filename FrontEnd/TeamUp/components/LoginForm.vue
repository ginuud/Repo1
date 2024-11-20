<template>
    <div
        class="space-y-4"
    >
      <UForm :state="user" class="space-y-4" @submit="submit">
        <UFormGroup label="User" name="user">
          <UInput v-model="user.username"/>
        </UFormGroup>
  
        <UFormGroup label="Password" name="password">
          <UInput v-model="user.password" type="password"/>
        </UFormGroup>
  
        <UButton type="submit">
          Submit
        </UButton>
      </UForm>
    </div>
  </template>
  
  <script setup lang="ts">
  import {ref} from 'vue';
import { useAuth } from '~/composables/useAuth';
import type { User } from '~/types/user';
  
  const auth = useAuth();
  const user: User = {username: '', password: ''};
  
  let showError = ref(false);
  
  const submit = async () => {
    showError.value = !(await auth.logIn(user));
  
    await navigateTo('/players');
  };
</script>
  