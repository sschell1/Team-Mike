<template>
  <div class="content">
    <navbar />
    <div class="team-mike">
      <div>
        <h2>Searra Schell</h2>
        <img src="../../public/serras.jpg" />
        <p>https://www.linkedin.com/in/searra-schell/</p>
      </div>
      <div>
        <h2>Shabnam Ahmed</h2>
        <img src="../../public/Shabnama.jpg" />
        <p>https://www.linkedin.com/in/shabnam-ahmed/</p>
      </div>
      <div>
        <h2>Gene Huh</h2>
        <img src="../../public/geneh.jpg" />
        <p>https://www.linkedin.com/in/geneshuh/</p>
      </div>
      <div>
        <h2>Paul Curtin</h2>
        <img src="../../public\paulc.jpg" />
        <p>https://www.linkedin.com/in/curtinpaul1/</p>
      </div>
    </div>
  </div>
</template>

<script>
import Navbar from "@/components/Navbar";
import auth from "../auth";

export default {
  components: {
    Navbar
  },
  data() {
    return {
      userRole: ""
    };
  },
  created() {
    auth.getToken();
  }
};
</script>
<style>
.team-mike {
  display: flex;
  justify-content: space-evenly;
  flex-wrap: wrap;
  height: 60vh;
  ;
}
.team-mike div {
  flex-basis: 35%;
}

h2 {
  color: #f0ab00;
}
img {
  width: 30%;
  height: auto;
  margin-left: 0px;
}
</style>