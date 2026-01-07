<template>
  <!-- Messenger Button -->
  <div
    v-if="!isOpen"
    class="fixed bottom-6 right-6 z-[9999] cursor-pointer"
    @click="toggleChat"
  >
    <div
      class="relative w-14 h-14 bg-blue-600 text-white rounded-full shadow-xl
             flex items-center justify-center hover:scale-105 transition-transform"
    >
      <!-- Icon -->
      <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" class="w-7 h-7" fill="white">
        <path
          d="M12 2c5.523 0 10 4.477 10 10s-4.477 10-10 10
             a9.96 9.96 0 0 1-4.863-1.26l-.305-.178-3.032.892
             a1.01 1.01 0 0 1-1.28-1.145l.026-.109.892-3.032
             A9.96 9.96 0 0 1 2 12C2 6.477 6.477 2 12 2"
        />
      </svg>

      <span
        v-if="unreadCount > 0"
        class="absolute -top-1 -right-1 bg-red-600 text-white
               text-[10px] min-w-[18px] h-[18px]
               flex items-center justify-center rounded-full px-1"
      >
        {{ unreadCount > 99 ? '99+' : unreadCount }}
      </span>
    </div>
  </div>

  <!-- Chat Window -->
  <transition name="chat">
    <div
      v-if="isOpen"
      class="fixed bottom-24 right-6 z-50 w-80 bg-white rounded-lg shadow-xl overflow-hidden"
    >
      <!-- Header -->
      <div class="bg-blue-600 text-white p-3 flex justify-between items-center">
        <span class="font-semibold">Messenger</span>
        <button @click="toggleChat">✕</button>
      </div>
<div class="p-3 h-64 overflow-y-auto flex flex-col gap-2 messages-container">
  <div
    v-for="(msg, index) in messages"
    :key="index"
    class=""
    :class="msg.isSender ? 'ml-auto' : 'mr-auto'"
  >
    <div
      class="px-3 py-2 rounded-lg max-w-[75%] break-words inline-block"
      :class="{
        'bg-blue-600 text-white': msg.isSender,
        'bg-yellow-200 text-yellow-900': !msg.isSender && msg.senderRole === 'Admin',
        'bg-gray-200 text-gray-900': !msg.isSender && msg.senderRole !== 'Admin'
      }"
    >
      <div v-if="!msg.isSender" class="text-[11px] font-semibold mb-0.5 text-gray-600">
        {{ msg.displaySender }}
      </div>
      <div class="text-sm">{{ msg.content }}</div>
      <div v-if="msg.createdAt" class="text-[10px] opacity-70 mt-1 text-right">
        {{ new Date(msg.createdAt).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }) }}
      </div>
    </div>
  </div>
</div>





      <!-- Input -->
      <div class="border-t p-2 flex items-center gap-2">
        <input
          v-model="message"
          type="text"
          placeholder="Type a message..."
          class="flex-1 text-sm px-3 py-2 border rounded focus:outline-none"
          @keyup.enter="sendMessage"
        />
        <button @click="sendMessage" class="p-2 rounded-full bg-blue-600 hover:bg-blue-700">
          <svg xmlns="http://www.w3.org/2000/svg" class="w-5 h-5 text-white" viewBox="0 0 24 24" fill="currentColor">
            <path d="M2 21l21-9L2 3v7l15 2-15 2v7z" />
          </svg>
        </button>
      </div>
    </div>
  </transition>
</template>

<script setup lang="ts">
import { ref, onMounted, onBeforeUnmount, nextTick } from 'vue'
import * as signalR from '@microsoft/signalr'
import { useSessionStore } from '#imports'
import { messengerStore } from '~/stores/chat'

interface ChatMessage {
  content: string
  createdAt?: string
  displaySender?: string
  isSender: boolean
  senderRole?: string // Admin or other
}

const sessionStore = useSessionStore()
const messengerStores = messengerStore()

const session = ref<any>(null)
const isOpen = ref(false)
const message = ref('')
const messages = ref<ChatMessage[]>([])
const unreadCount = ref(0)

const adminUserId = 87
let connection: signalR.HubConnection

const scrollToBottom = () => {
  nextTick(() => {
    const el = document.querySelector('.messages-container') as HTMLElement
    if (el) el.scrollTop = el.scrollHeight
  })
}

// Load messages from backend
const loadChatHistory = async () => {
  const userId = session.value?.userId
  if (!userId) return

  const response = await messengerStores.chatMessageFetch(userId)
  const data = response.messages || []

  messages.value = data.map((msg: any) => ({
    isSender: msg.senderId === session.value.userId,
    content: msg.content,
    createdAt: msg.createdAt,
    displaySender: msg.senderName,
    senderRole: msg.senderRole
  }))

  scrollToBottom()
}

const toggleChat = async () => {
  isOpen.value = !isOpen.value
  if (isOpen.value) {
    unreadCount.value = 0
    await loadChatHistory()
  }
}

// Send new message via SignalR
const sendMessage = async () => {
  if (!message.value.trim()) return
  if (connection.state !== signalR.HubConnectionState.Connected) return

  await connection.invoke('SendMessage', adminUserId, message.value)
  message.value = ''
}

onMounted(async () => {
  await sessionStore.getSession()
  session.value = sessionStore.session

  const token = localStorage.getItem('token')
  if (!token) return

  connection = new signalR.HubConnectionBuilder()
    .withUrl('http://localhost:5080/hubs/messenger', {
      accessTokenFactory: () => token
    })
    .withAutomaticReconnect()
    .build()

  // Listen for new messages
  connection.on('ReceiveMessage', (msg) => {
    const userId = session.value?.userId
    const isSender = msg.senderId === userId

    messages.value.push({
      isSender,
      content: msg.content,
      createdAt: msg.createdAt,
      displaySender: msg.senderName,
      senderRole: msg.senderRole
    })

    if (!isOpen.value && !isSender) unreadCount.value++
    scrollToBottom()
  })

  await connection.start()
})

onBeforeUnmount(() => {
  connection?.stop()
})
</script>

<style scoped>
.chat-enter-active,
.chat-leave-active {
  transition: all 0.25s ease;
}
.chat-enter-from,
.chat-leave-to {
  opacity: 0;
  transform: translateY(10px);
}
</style>
