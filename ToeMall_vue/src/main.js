import Vue from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'
import request from '@/utils/request.js'
import 'element-ui/lib/theme-chalk/index.css'
import {
  Button, Drawer, Container, Aside, Menu,
  Submenu, MenuItemGroup, MenuItem, Header, Dropdown,
  DropdownMenu, DropdownItem, Main, Table, TableColumn,
  Message, Footer, InfiniteScroll, Col, Card, Form, FormItem, Input, Dialog,
  Row, InputNumber, Pagination, Avatar, Tooltip, Select, Option, Image
} from 'element-ui'

Vue.config.productionTip = false

Vue.component(Button.name, Button)
Vue.component(Drawer.name, Drawer)
Vue.component(Container.name, Container)
Vue.component(Aside.name, Aside)
Vue.component(Menu.name, Menu)
Vue.component(Submenu.name, Submenu)
Vue.component(MenuItemGroup.name, MenuItemGroup)
Vue.component(MenuItem.name, MenuItem)
Vue.component(Header.name, Header)
Vue.component(Dropdown.name, Dropdown)
Vue.component(DropdownMenu.name, DropdownMenu)
Vue.component(DropdownItem.name, DropdownItem)
Vue.component(Main.name, Main)
Vue.component(Table.name, Table)
Vue.component(TableColumn.name, TableColumn)
Vue.component(Card.name, Card)
Vue.component(Form.name, Form)
Vue.component(FormItem.name, FormItem)
Vue.component(Input.name, Input)
Vue.component(Footer.name, Footer)
Vue.component(Col.name, Col)
Vue.use(Dialog)
Vue.use(Row)
Vue.component(Pagination.name, Pagination)
Vue.component(InputNumber.name, InputNumber)
Vue.component(Avatar.name, Avatar)
Vue.component(Tooltip.name, Tooltip)
Vue.component(Select.name, Select)
Vue.component(Option.name, Option)
Vue.component(Image.name, Image)

Vue.use(InfiniteScroll)

Vue.prototype.$message = Message
Vue.prototype.$request = request

// 添加 currency 过滤器
Vue.filter('currency', function (value) {
  if (!value) return ''
  return '¥' + parseFloat(value).toFixed(2)
})

new Vue({
  router,
  store,
  render: h => h(App)
}).$mount('#app')
