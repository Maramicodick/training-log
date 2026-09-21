<script setup lang="ts">
import { onMounted, ref } from 'vue'

type Activity = {
  id: string
  date: string
  title: string
  notes: string | null
}

function todayLocal(): string {
  const d = new Date()
  const month = String(d.getMonth() + 1).padStart(2, '0')
  const day = String(d.getDate()).padStart(2, '0')
  return `${d.getFullYear()}-${month}-${day}`
}

async function readError(response: Response): Promise<string> {
  const text = await response.text()
  try {
    const parsed = JSON.parse(text)
    if (typeof parsed === 'string') {
      return parsed
    }
  } catch {
    /* body is plain text */
  }
  return text || 'Request failed'
}

const activities = ref<Activity[]>([])
const date = ref(todayLocal())
const title = ref('')
const notes = ref('')
const error = ref('')
const loading = ref(false)

async function loadActivities() {
  const response = await fetch('/api/activities')
  if (!response.ok) {
    error.value = await readError(response)
    return
  }
  activities.value = await response.json()
}

async function addActivity() {
  error.value = ''
  loading.value = true
  try {
    const response = await fetch('/api/activities', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        date: date.value,
        title: title.value,
        notes: notes.value
      })
    })
    if (!response.ok) {
      error.value = await readError(response)
      return
    }
    title.value = ''
    notes.value = ''
    date.value = todayLocal()
    await loadActivities()
  } finally {
    loading.value = false
  }
}

async function deleteActivity(id: string) {
  error.value = ''
  const response = await fetch(`/api/activities/${id}`, { method: 'DELETE' })
  if (!response.ok) {
    error.value = await readError(response)
    return
  }
  await loadActivities()
}

onMounted(() => {
  loadActivities()
})
</script>

<template>
  <main>
    <h1>Completed activities</h1>

    <form @submit.prevent="addActivity">
      <label>
        Date
        <input v-model="date" type="date" required />
      </label>
      <label>
        Title
        <input v-model="title" type="text" required />
      </label>
      <label>
        Notes
        <textarea v-model="notes" rows="3" />
      </label>
      <button type="submit" :disabled="loading">Add</button>
    </form>

    <p v-if="error" class="error">{{ error }}</p>

    <p v-if="activities.length === 0">No activities yet.</p>
    <ul v-else>
      <li v-for="activity in activities" :key="activity.id">
        <strong>{{ activity.date }}</strong>
        {{ activity.title }}
        <span v-if="activity.notes"> — {{ activity.notes }}</span>
        <button type="button" @click="deleteActivity(activity.id)">Delete</button>
      </li>
    </ul>
  </main>
</template>
