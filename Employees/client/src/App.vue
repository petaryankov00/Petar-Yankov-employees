<template>
  <div class="container">
    <div class="row mt-5 text-center">
      <form @submit.prevent="uploadFile">
        <div class="form-group">
          <label for="fileUpload">Select a file to upload:</label>
          <input type="file" class="form-control" @change="setFile($event)" />
        </div>
        <button type="submit" class="btn btn-primary">Upload</button>
      </form>
    </div>
    <div class="row text-center mt-5">
      <div class="card">
        <div class="card-body">
          <h5 class="card-title">Longest pair of employees worked together</h5>
          <table class="table" v-if="longestPair">
            <thead>
              <tr>
                <th>EmployeeId#1</th>
                <th>EmployeeId#2</th>
                <th>WorkedDays</th>
              </tr>
            </thead>
            <tbody>
              <tr>
                <td>{{ longestPair.workerId }}</td>
                <td>{{ longestPair.coWorkerId }}</td>
                <td>{{ longestPair.workedDays }}</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
    <div class="row mt-5">
      <div>
        <h2 class="text-center">Employee List</h2>
        <table class="table">
          <thead>
            <tr>
              <th>EmployeeId#1</th>
              <th>EmployeeId#2</th>
              <th>ProjectId</th>
              <th>WorkedDays</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="ep in employeeProjects">
              <td>{{ ep.workerId }}</td>
              <td>{{ ep.coWorkerId }}</td>
              <td>{{ ep.projectId }}</td>
              <td>{{ ep.workedDays }}</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  name: "App",
  data() {
    return {
      employeeProjects: [],
      file: null,
      longestPair: null,
      errorMessage: "Test"
    }
  },
  methods: {
    setFile(event) {
      this.file = event.target.files[0];
    },
    async uploadFile() {
      if (this.file) {
        const formData = new FormData();
        formData.append('document', this.file);
        try {
          const response = await axios.post('https://localhost:7018/api/Employees/get-longest-pair', formData, {
            headers: {
              'Content-Type': 'multipart/form-data',
              'Access-Control-Allow-Origin': '*'
            }
          });
          this.longestPair = response.data;
          const commonEmployeeProjects = await axios.post('https://localhost:7018/api/Employees/get-employeeProjects', formData, {
            headers: {
              'Content-Type': 'multipart/form-data',
              'Access-Control-Allow-Origin': '*'
            }
          });
          this.employeeProjects = commonEmployeeProjects.data;
        }
        catch (error) {
          alert(`Upload failed: ${error.response.data}`);
          this.employeeProjects = [];
          this.longestPair = null;
        }
      }
    }
  }

};
</script>
