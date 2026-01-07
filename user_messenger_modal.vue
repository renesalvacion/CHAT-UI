<template>
  <transition name="chat">
    <div v-if="isOpen" class="fixed bottom-24 right-6 z-50 w-80 bg-white rounded-lg shadow-xl overflow-hidden">
      <div class="bg-blue-600 text-white p-3 flex justify-between items-center">
        <span class="font-semibold">Messenger</span>
        <button @click="closeModal">✕</button>
      </div>

      <div class="p-3 h-64 overflow-y-auto flex flex-col gap-2 messages-container">
        <div v-for="(msg, index) in messages" :key="index" class="max-w-[75%] flex flex-col"
             :class="{
               'self-end items-end': msg.sender === 'me',
               'self-start items-start': msg.sender === 'other',
               'self-center items-center': msg.sender === 'server',
             }">
<div
  class="px-3 py-2 rounded-lg"
  :class="{
    'bg-blue-600 text-white': msg.sender === 'me',
    'bg-gray-200 text-gray-900': msg.sender === 'other',
    'bg-green-100 text-green-600 text-center': msg.sender === 'server',
    'bg-yellow-200 text-yellow-800': msg.sender === 'admin', // new for admin
  }"
>

            <div v-if="msg.sender !== 'me' && msg.sender !== 'server' && isAdmin"
                 class="text-[11px] font-semibold mb-0.5 text-gray-600">
              {{ msg.displaySender || msg.sender }}
            </div>
            <div class="text-sm break-words">{{ msg.content }}</div>
            <div v-if="msg.createdAt" class="text-[10px] opacity-70 mt-1 text-right">
              {{ new Date(msg.createdAt).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }) }}
            </div>
          </div>
        </div>
      </div>

      <div class="border-t p-2 flex items-center gap-2">
        <input v-model="message" type="text" placeholder="Type a message..."
               class="flex-1 text-sm px-3 py-2 border rounded focus:outline-none"
               @keyup.enter="sendMessage"/>
        <button @click="sendMessage" class="p-2 rounded-full bg-blue-600 hover:bg-blue-700">
          <svg xmlns="http://www.w3.org/2000/svg" class="w-5 h-5 text-white" viewBox="0 0 24 24"
               fill="currentColor">
            <path d="M2 21l21-9L2 3v7l15 2-15 2v7z"/>
          </svg>
        </button>
      </div>
    </div>
  </transition>
</template>

<script setup lang="ts">
import { ref, onMounted, onBeforeUnmount, nextTick, computed, watch } from "vue";
import * as signalR from "@microsoft/signalr";
import { useSessionStore } from "#imports";

interface ChatMessage {
  sender: "me" | "other" | "server" | "admin"; // added admin
  content: string;
  createdAt?: string;
  displaySender?: string;
}


const props = defineProps<{ crudId: number }>();
const emit = defineEmits<{ (e: 'close'): void }>();

const isOpen = ref(true);
const message = ref("");
const messages = ref<ChatMessage[]>([]);
let connection: signalR.HubConnection;

const sessionStore = useSessionStore();
const session = ref(sessionStore.session);

// Adjust isAdmin check to match JWT role claim
const isAdmin = computed(() =>
  session.value?.claims?.some(c =>
    c.type === 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role' && c.value === 'Admin')
);

const closeModal = () => { isOpen.value = false; emit("close"); };

const scrollToBottom = () => {
  nextTick(() => {
    const el = document.querySelector(".messages-container") as HTMLElement;
    if (el) el.scrollTop = el.scrollHeight;
  });
};
const recipientId = props.crudId?.toString();
const sendMessage = async () => {
  if (!message.value.trim() || connection.state !== signalR.HubConnectionState.Connected) return;
  const recipientId = props.crudId?.toString();

  if (!recipientId) return;

  try {
    await connection.invoke("SendMessage", parseInt(recipientId), message.value);
    message.value = ""; // clear input
    scrollToBottom();
  } catch (err) {
    console.error("Send message failed:", err);
  }
};


onMounted(async () => {
  const token = localStorage.getItem("token");
  if (!token) return;

  connection = new signalR.HubConnectionBuilder()
    .withUrl("http://localhost:5080/hubs/messenger", {
        accessTokenFactory: () => localStorage.getItem("token") || ""
    })
    .withAutomaticReconnect()
    .build();

connection.on("ReceiveMessage", (msg) => {
  const currentUserId = session.value?.userId?.toString();
  const isMe = msg.senderId?.toString() === currentUserId;
  const isAdminSender = msg.senderRole === "Admin"; // use correct casing

  messages.value.push({
    sender: isMe ? "me" : isAdminSender ? "admin" : "other",
    content: msg.content,
    createdAt: msg.createdAt,
    displaySender: isMe ? undefined : msg.senderName
  });

  scrollToBottom();
});




  connection.onclose(() => {
    messages.value.push({ sender: "server", content: "🔌 Disconnected" });
  });

  try {
    await connection.start();
    messages.value.push({ sender: "server", content: "💬 Connected" });
  } catch (err) {
    messages.value.push({ sender: "server", content: "⚠️ Connection failed" });
    console.error(err);
  }
});

onBeforeUnmount(() => connection?.stop());

watch(() => sessionStore.session, (newSession) => { session.value = newSession; });
</script>

<style scoped>
.chat-enter-active, .chat-leave-active { transition: all 0.25s ease; }
.chat-enter-from, .chat-leave-to { opacity: 0; transform: translateY(10px); }
</style>
