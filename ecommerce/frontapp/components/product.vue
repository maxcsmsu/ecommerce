<!-- Please remove this file from your project -->
<template>
  <b-container>

    <b-card zlass="text-center">
      <b-form-input v-model="productsearch" placeholder="Enter product name" @keydown.native="input_keydown" />
      <b-card-text>Value: {{ productsearch }}</b-card-text>
    </b-card>
    <b-card header="Products" class="text-center">
      <b-card v-for="(product, index) in products" :key="index">
        <b-card-text>Name: {{ product.name }}</b-card-text>
      </b-card>
    </b-card>
  </b-container>
</template>

<script>
  import axios from '@nuxtjs/axios'
  export default {
    name: 'Product',
    data() {
      return {
        productsearch: '',
        products: [],
        productlist: [
          { product_id: 1, product_name: "สเปรย์ดับเพลิง", amount: 690, stock: 500 },
          { product_id: 2, product_name: "ยาแนวรอยต่อ", amount: 150, stock: 500 },
          { product_id: 3, product_name: "ชั้นรถเข็นเล็ก", amount: 990, stock: 500 },
          { product_id: 4, product_name: "ฉนวนกันความร้อน", amount: 1200, stock: 500 },
          { product_id: 5, product_name: "เทปพันท่อ", amount: 50, stock: 500 },
          { product_id: 6, product_name: "ซิลิโคน", amount: 90, stock: 500 },
          { product_id: 7, product_name: "สายยาง", amount: 750, stock: 500 }
        ],

      }
    },
    async mounted() {
      await this.fetchProduct();
    },
    methods: {
      async input_keydown(event) {
        if (event.which === 13) {

          const params = new URLSearchParams();
          params.append('name', this.productsearch);
          console.log('get');
          this.products = await this.$axios.$get('api/Product', { params: params });
          console.log(this.products);  
          //this.products = this.productlist.filter(item => item.product_name.toLowerCase().indexOf(this.productsearch.toLowerCase()) >= 0)
        }
      },
      async fetchProduct() {
        this.products =await this.$axios.$get('api/Product');
      }
    }
  }
</script>
