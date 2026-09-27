<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'

type Activity = {
  id: string
  date: string
  title: string
  notes: string | null
  sport: string
  durationMinutes: number | null
  distanceKilometers: number | null
}

const sports = [
  { value: 'Run', label: 'Run' },
  { value: 'RoadRide', label: 'Road ride' },
  { value: 'Swim', label: 'Swim' },
  { value: 'Strength', label: 'Strength' },
  { value: 'Other', label: 'Other' }
]

function todayLocal(): string {
  const d = new Date()
  const month = String(d.getMonth() + 1).padStart(2, '0')
  const day = String(d.getDate()).padStart(2, '0')
  return `${d.getFullYear()}-${month}-${day}`
}

function sportLabel(value: string): string {
  return sports.find(sport => sport.value === value)?.label ?? value
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
const sport = ref('')
const duration = ref('')
const distance = ref('')
const editingId = ref<string | null>(null)
const error = ref('')
const loading = ref(false)

const showDetails = computed(() => sport.value !== '')
const showDistance = computed(() => showDetails.value && sport.value !== 'Strength')

function resetForm() {
  editingId.value = null
  date.value = todayLocal()
  title.value = ''
  notes.value = ''
  sport.value = ''
  duration.value = ''
  distance.value = ''
}

function requestBody() {
  return {
    date: date.value,
    title: title.value,
    notes: notes.value,
    sport: sport.value,
    durationMinutes: duration.value === '' ? null : Number(duration.value),
    distanceKilometers: showDistance.value && distance.value !== '' ? Number(distance.value) : null
  }
}

async function loadActivities() {
  const response = await fetch('/api/activities')
  if (!response.ok) {
    error.value = await readError(response)
    return
  }
  activities.value = await response.json()
}

function editActivity(activity: Activity) {
  editingId.value = activity.id
  date.value = activity.date
  title.value = activity.title
  notes.value = activity.notes ?? ''
  sport.value = activity.sport
  duration.value = activity.durationMinutes == null ? '' : String(activity.durationMinutes)
  distance.value = activity.distanceKilometers == null ? '' : String(activity.distanceKilometers)
  error.value = ''
}

async function saveActivity() {
  error.value = ''
  loading.value = true
  const editing = editingId.value
  try {
    const response = await fetch(editing ? `/api/activities/${editing}` : '/api/activities', {
      method: editing ? 'PATCH' : 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(requestBody())
    })
    if (!response.ok) {
      error.value = await readError(response)
      return
    }
    resetForm()
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
  if (editingId.value === id) {
    resetForm()
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

    <form @submit.prevent="saveActivity">
      <label>
        Sport
        <select v-model="sport" required>
          <option value="" disabled>Choose a sport</option>
          <option v-for="item in sports" :key="item.value" :value="item.value">
            {{ item.label }}
          </option>
        </select>
      </label>

      <template v-if="showDetails">
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
        <label>
          Duration (minutes)
          <input v-model="duration" type="number" min="1" step="1" />
        </label>
        <label v-if="showDistance">
          Distance (km)
          <input v-model="distance" type="number" min="0.01" step="0.01" />
        </label>
      </template>

      <button type="submit" :disabled="loading">{{ editingId ? 'Save' : 'Add' }}</button>
      <button v-if="editingId" type="button" @click="resetForm">Cancel</button>
    </form>

    <p v-if="error" class="error">{{ error }}</p>

    <p v-if="activities.length === 0">No activities yet.</p>
    <ul v-else>
      <li v-for="activity in activities" :key="activity.id">
        <strong>{{ activity.date }}</strong>
        {{ sportLabel(activity.sport) }}
        {{ activity.title }}
        <span v-if="activity.durationMinutes != null"> — {{ activity.durationMinutes }} min</span>
        <span v-if="activity.distanceKilometers != null"> — {{ activity.distanceKilometers }} km</span>
        <span v-if="activity.notes"> — {{ activity.notes }}</span>
        <button type="button" @click="editActivity(activity)">Edit</button>
        <button type="button" @click="deleteActivity(activity.id)">Delete</button>
      </li>
    </ul>
  </main>
</template>
