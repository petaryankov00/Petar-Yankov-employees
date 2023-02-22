import { createApp } from 'vue'
import App from './App.vue'
import VueAxios from 'vue-axios'
import axios from 'axios';

// import './assets/main.css'
import "bootstrap/dist/css/bootstrap.min.css"

const app = createApp(App);
app.use(VueAxios, axios);
app.mount('#app');
