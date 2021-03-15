<template>
  <div id="app">
    <header>
      <title>Product Approval</title>
    </header>
    <router-link to="home" >
    <img
      src="../public\Sarnova-Logo_2017_RGB-1.png"
      id="logo"
    />
    </router-link>
    <router-view />
    <div id="footer">
      <p>
        <a href="http://www.sarnova.com/privacy-policy/">Privacy Policy</a>
      </p>
      <div class="copy">© 2020, Sarnova, Inc. All Rights Reserved</div>
      <div class="social">
        <a href="https://www.facebook.com/SarnovaHC/" target="_blank" title="Visit Us On Facebook">
          <img
            src="http://www.sarnova.com/wp-content/plugins/acurax-social-media-widget/images/themes/25/facebook.png"
            style="border:0px;"
            alt="Visit Us On Facebook"
          />
        </a>
        <a
          href="https://www.linkedin.com/company/sarnova"
          target="_blank"
          title="Visit Us On Linkedin"
        >
          <img
            src="http://www.sarnova.com/wp-content/plugins/acurax-social-media-widget/images/themes/25/linkedin.png"
            style="border:0px;"
            alt="Visit Us On Linkedin"
          />
        </a>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  computed: {
        isAdmin () {
            return this.currentUser && this.currentUser.role === "Admin";
        }
    }
};
</script>

<style>
body {
  height: 100vh;
  width: 100vw;
  background-color: #ffffff;
  background-repeat: repeat-all;
  background-position: center center;
  background-attachment: scroll;
  font-family: "Open sans", Arial, sans-serif;
  font-weight: 400;
  font-size: 14px;
  line-height: 22px;
  margin: 0;
}
.content {
  background-color: #004165;
  color: #ffffff;
  padding: 5px 20px;
  font-family: "Open sans", Arial, sans-serif;
  font-weight: 400;
  font-size: 14px;
  line-height: 22px;
}
#footer {
  position: fixed;
  bottom: 0;
  width: 100%;
  padding: 1px 10px 10px 20px;
  display: block;
  color: #d9d9d9;
  background-color: #00324c;
}
button {
  background-color: transparent;
  border: 1px solid #bbbbbb;
  border-radius: 25px;
  color: #ffffff;
  cursor: pointer;
  line-height: 18px;
  margin: 1% 1% 1% 1%;
  padding: 5px 10px;
  font-weight: bold;
}
button:hover {
  background-color: #f0ab00;
  color: black;
}
img {
  width: 30%;
  height: auto;
  margin-left: 20px;
  padding: 0;
}
a,
a:visited {
  text-decoration: none;
  color: #d9d9d9;
}
.social {
  background-color: #00324c;
  text-align: right;
  padding-right: 20px;
}
.social img {
  width: 32px;
  margin-left: 1px;
}
hr {
  color: #f0ab00;
}

#logo {
  padding: 10px 0 10px 20px;
}
</style>