import { defineStore } from 'pinia';

import axios from 'axios'

export  const messengerStore = defineStore("chat", {
    state: () => ({
        message: [] as any[]
    }),
    actions:{
        async  chatMessageFetch(userId: number){
            try {
                 const token = localStorage.getItem("token"); // should match what backend expects
const res = await axios.get(`http://localhost:5080/api/chat/${userId}`, {
  headers: { Authorization: `Bearer ${token}` }
})


                if(res.status === 200){
                    
                    this.message = res.data
                    console.log("Message", res.data)
                    return this.message
                }else{
                     console.log("Message", res.data)
                    this.message = []
                    return this.message
                }
            } catch (error: any) {

                console.log("Error: " + error.Message);
                this.message = []
                return this.message
            }
        }
    }
}) 

