<template>
  <sidebarAdmin :open="sidebarOpen" @toggle="sidebarOpen = !sidebarOpen" />
  <!-- Main Content -->
  <main
    class="flex-1 p-8 transition-all duration-300"
    :class="sidebarOpen ? 'ml-64' : 'ml-20'"
  >
    <div class="view pt-3 p-20">
      <h1 class="text-xl font-bold">Admin View Reservation</h1>
      <div
        class="analytics grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6 p-6"
      >
        <!-- Reservation -->
        <div
          class="cards bg-gradient-to-br from-blue-100 to-blue-300 text-blue-700 p-6 rounded-2xl shadow-lg flex items-center gap-4 hover:scale-105 transition transform cursor-pointer"
        >
          <span class="text-4xl">
            <!-- Calendar Check SVG -->
            <svg
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              stroke="currentColor"
              stroke-width="2"
              viewBox="0 0 24 24"
              class="w-10 h-10"
            >
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                d="M8 7V3m8 4V3m-9 8h10M3 21h18a2 2 0 002-2V7a2 2 0 00-2-2H3a2 2 0 00-2 2v12a2 2 0 002 2z"
              />
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                d="M16 12l-4 4-2-2"
              />
            </svg>
          </span>
          <h1 class="text-xl font-bold">Reservation</h1>
        </div>

        <!-- Review -->
        <div
          class="cards bg-gradient-to-br from-purple-100 to-purple-300 text-purple-700 p-6 rounded-2xl shadow-lg flex items-center gap-4 hover:scale-105 transition transform cursor-pointer"
        >
          <span class="text-4xl">
            <!-- Star SVG -->
            <svg
              xmlns="http://www.w3.org/2000/svg"
              fill="currentColor"
              viewBox="0 0 24 24"
              class="w-10 h-10"
            >
              <path
                d="M12 .587l3.668 7.568 8.332 1.151-6.064 5.861 1.432 8.277L12 18.897l-7.368 3.848 1.432-8.277-6.064-5.861 8.332-1.151z"
              />
            </svg>
          </span>
          <h1 class="text-xl font-bold">Review</h1>
        </div>

        <!-- Available Rooms -->
        <div
          class="cards bg-gradient-to-br from-green-100 to-green-300 text-green-600 p-6 rounded-2xl shadow-lg flex items-center gap-4 hover:scale-105 transition transform cursor-pointer"
        >
          <span class="text-4xl">
            <!-- Bed SVG -->
            <svg
              xmlns="http://www.w3.org/2000/svg"
              fill="currentColor"
              viewBox="0 0 24 24"
              class="w-10 h-10"
            >
              <path
                d="M21 10V7a3 3 0 00-3-3H6a3 3 0 00-3 3v3H0v7h2v-3h20v3h2v-7h-3zM6 7h12v3H6V7z"
              />
            </svg>
          </span>
          <h1 class="text-xl font-bold">No. Available Room</h1>
        </div>

        <!-- Sales -->
        <div
          class="cards bg-gradient-to-br from-amber-100 to-amber-300 text-amber-500 p-6 rounded-2xl shadow-lg flex items-center gap-4 hover:scale-105 transition transform cursor-pointer"
        >
          <span class="text-4xl">
            <!-- Cash / Money SVG -->
            <svg
              xmlns="http://www.w3.org/2000/svg"
              fill="currentColor"
              viewBox="0 0 24 24"
              class="w-10 h-10"
            >
              <path
                d="M21 7H3c-1.1 0-2 .9-2 2v6c0 1.1.9 2 2 2h18c1.1 0 2-.9 2-2V9c0-1.1-.9-2-2-2zm-9 7c-2.21 0-4-1.79-4-4s1.79-4 4-4 4 1.79 4 4-1.79 4-4 4z"
              />
              <circle cx="12" cy="10" r="2" />
            </svg>
          </span>
          <h1 class="text-xl font-bold">Sales</h1>
        </div>
      </div>

      <div class="admin_table mt-6 flex justify-center flex-col">
        <!-- Card Container for the Table -->
        <div class="w-full p-4 rounded-lg shadow-md">
          <!-- Table with overflow handling -->
          <div class="overflow-x-auto bg-white rounded-lg shadow-md">
            <table class="min-w-full divide-y table-auto">
              <!-- Table Header -->
              <thead class="bg-gray-50">
                <tr>
                  <th
                    class="px-4 py-3 text-left text-sm font-bold text-gray-600 uppercase tracking-wider"
                  >
                    #
                  </th>
                  <th
                    class="px-4 py-3 text-left text-sm font-bold text-gray-600 uppercase tracking-wider"
                  >
                    Name
                  </th>

                  <th
                    class="px-4 py-3 text-sm font-bold text-gray-600 uppercase tracking-wider"
                  >
                    No. Reservation
                  </th>
                  <th
                    class="px-4 py-3 text-center text-sm font-bold text-gray-600 uppercase tracking-wider"
                  >
                    Status
                  </th>
                  <th
                    class="px-4 py-3 text-sm font-bold text-gray-600 uppercase tracking-wider"
                  >
                    Actions
                  </th>
                </tr>
              </thead>

              <tbody class="divide-y divide-gray-100">
                <tr
                  v-for="(item, index) in appointmentStore.AdminViewAppointment"
                  :key="item.id"
                  class="hover:bg-gray-50 transition-colors cursor-pointer"
                >
                  <td class="px-4 py-3 text-sm text-gray-700">
                    {{ item.appointmentId }}
                  </td>
                  <td class="px-4 py-3 text-sm text-gray-700 font-bold">
                    {{ item.crudsName }}
                  </td>

                  <td class="px-4 py-3 text-sm text-gray-700 text-center">
                    {{ item.reservationCount }}
                  </td>
                  <td class="px-4 py-3 text-sm text-gray-700 text-center">
                    {{ item.status }}
                  </td>
                  <td class="px-4 py-3 text-sm text-gray-700">
                    <div class="flex justify-center gap-2">
                      <!-- View -->
                      <button
                        class="cursor-pointer bg-blue-500 hover:bg-blue-600 text-white px-3 py-1 rounded transition"
                        @click="ViewAppointment(item.crudId)"
                      >
                        <svg
                          xmlns="http://www.w3.org/2000/svg"
                          class="h-5 w-5"
                          fill="currentColor"
                        >
                          <path
                            d="M12 16q1.875 0 3.188-1.312T16.5 11.5t-1.312-3.187T12 7T8.813 8.313T7.5 11.5t1.313 3.188T12 16m0-1.8q-1.125 0-1.912-.788T9.3 11.5t.788-1.912T12 8.8t1.913.788t.787 1.912t-.787 1.913T12 14.2m0 4.8q-3.65 0-6.65-2.037T1 11.5q1.35-3.425 4.35-5.462T12 4t6.65 2.038T23 11.5q-1.35 3.425-4.35 5.463T12 19m0-2q2.825 0 5.188-1.487T20.8 11.5q-1.25-2.525-3.613-4.012T12 6T6.813 7.488T3.2 11.5q1.25 2.525 3.613 4.013T12 17"
                          />
                        </svg>
                      </button>

                      <!-- Edit -->
                      <button
                        class="cursor-pointer bg-pink-500 hover:bg-pink-600 text-white px-3 py-1 rounded transition"
                        @click="openAdminEditModal()"
                      >
                        <svg
                          xmlns="http://www.w3.org/2000/svg"
                          class="h-5 w-5"
                          fill="currentColor"
                        >
                          <path
                            d="M5 19h1.425L16.2 9.225L14.775 7.8L5 17.575zm-2 2v-4.25L16.2 3.575q.3-.275.663-.425t.762-.15t.775.15t.65.45L20.425 5q.3.275.438.65T21 6.4q0 .4-.137.763t-.438.662L7.25 21zM19 6.4L17.6 5zm-3.525 2.125l-.7-.725L16.2 9.225z"
                          />
                        </svg>
                      </button>
<!-- Messenger Button -->
<div
  class="cursor-pointer"
  @click="openMessengerModal(item.crudId)"
>
  <div
    class="w-10 h-10 bg-blue-600 text-white rounded-full shadow-xl flex items-center justify-center hover:scale-105 transition-transform"
  >
    <svg
      xmlns="http://www.w3.org/2000/svg"
      viewBox="0 0 24 24"
      class="w-5 h-5"
      fill="white"
    >
      <path
        d="M12 2c5.523 0 10 4.477 10 10s-4.477 10-10 10a9.96 9.96 0 0 1-4.863-1.26l-.305-.178l-3.032.892a1.01 1.01 0 0 1-1.28-1.145l.026-.109l.892-3.032A9.96 9.96 0 0 1 2 12C2 6.477 6.477 2 12 2"
      />
    </svg>
  </div>
</div>

                    </div>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </div>

      <!-- Pagination -->
      <div
        v-if="appointmentStore.totalPages > 1"
        class="mt-4 flex justify-center gap-2 items-center"
      >
        <!-- Prev button -->
        <button
          @click="prevPage"
          :disabled="appointmentStore.pageNumber === 1"
          class="cursor-pointer px-4 py-2 bg-gray-300 rounded hover:bg-gray-400 disabled:opacity-50"
        >
          Prev
        </button>

        <!-- Page number buttons -->
        <button
          v-for="page in appointmentStore.totalPages"
          :key="page"
          @click="goToPage(page)"
          :class="[
            'cursor-pointer px-4 py-2 rounded hover:bg-gray-400',
            appointmentStore.pageNumber === page
              ? 'bg-blue-600 text-white'
              : 'bg-gray-300',
          ]"
        >
          {{ page }}
        </button>

        <!-- Next button -->
        <button
          @click="nextPage"
          :disabled="
            appointmentStore.pageNumber === appointmentStore.totalPages
          "
          class="cursor-pointer px-4 py-2 bg-gray-300 rounded hover:bg-gray-400 disabled:opacity-50"
        >
          Next
        </button>
      </div>
    </div>
  </main>

  <admin_edit_modal v-if="adminEditModal" @close="adminEditModal = false" />
  <user_messenger_modal
  v-if="messengerModalOpen"
  :crud-id="selectedCrudId"
  @close="messengerModalOpen = false"
/>

</template>

<script setup lang="ts">
import sidebarAdmin from "~/components/sidebar-admin.vue";

import Navbar from "~/components/navbar.vue";
import { SetAppointment } from "~/stores/appointment";
import { useSessionStore } from "#imports";
import { onMounted, ref } from "vue";
import admin_edit_modal from "~/components/admin_edit_modal.vue";
import { useRouter } from "vue-router";

import user_cancel_modal from "~/components/user/user_cancel_modal.vue";
import user_edit_modal from "~/components/user/user_edit_modal.vue";
import user_review_modal from "~/components/user/user_review_modal.vue";
import user_messenger_modal from "~/components/user_messenger_modal.vue";

interface Session {
  id: number;
  role?: string | null;
  name?: string;
}

const sidebarOpen = ref(true);

const sessionStore = useSessionStore();
const appointmentStore = SetAppointment();

const session = ref<Session | null>(null);

const router = useRouter();
const appId = ref(0);
//modal
const isOpen = ref(false);
//user modal
const userEditModal = ref(false);
const userCancelModal = ref(false);
const userReviewModal = ref(false);
const adminEditModal = ref(false);

const selectedCrudId = ref<number | null>(null);
const messengerModalOpen = ref(false);

const openMessengerModal = (crudId: number) => {
  selectedCrudId.value = crudId;
  messengerModalOpen.value = true;
};

//modal function
const openModal = (appointmentId: number) => {
  appId.value = appointmentId;
  isOpen.value = true;
};

const openAdminEditModal = () => {
  adminEditModal.value = true;
};

//user modal function
const isUserEditModal = (appointmentId: number) => {
  appId.value = appointmentId;
  userEditModal.value = true;
};
const isUserCancelModal = (appointmentId: number) => {
  userCancelModal.value = true;
  appId.value = appointmentId;
  console.log("Appointment ID: " + appId.value);
};
const isUserReviewModal = () => {
  userReviewModal.value = true;
};

const ViewAppointment = (userId: number) => {
  router.push(`/admin/view-details/${userId}`);
};

const nextPage = () => {
  if (appointmentStore.pageNumber < appointmentStore.totalPages) {
    appointmentStore.ViewAdminCatalog(appointmentStore.pageNumber + 1);
  }
};

const prevPage = () => {
  if (appointmentStore.pageNumber > 1) {
    appointmentStore.ViewAdminCatalog(appointmentStore.pageNumber - 1);
  }
};

// Method to go to a specific page
const goToPage = (page: number) => {
  if (page >= 1 && page <= appointmentStore.totalPages) {
    appointmentStore.ViewAdminCatalog(page, appointmentStore.pageSize);
  }
};

onMounted(async () => {
  const s = await sessionStore.getSession();
  console.log("Session Loaded:", s);

  // Map userId to id if your interface expects it
  session.value = {
    email: s?.email,
    userId: s?.userId,
    role: s?.role,
  };

  if (session.value?.id) {
    appointmentStore.getAppointment(session.value.id);
  }

  if (session.value?.role === "Admin") {
    await appointmentStore.ViewAdminCatalog(1, 10);
  }
});
</script>
