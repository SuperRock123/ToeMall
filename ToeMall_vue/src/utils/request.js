import axios from 'axios'
import { Message, Loading } from 'element-ui'
import { getToken } from '@/utils/CookieUtil'

let loadingInstance = null // 存储 loading 实例

const instance = axios.create({
  baseURL: 'http://localhost:5227/api',
  // baseURL: 'http://10.70.61.97:5227/api',
  timeout: 5000
})

// 请求拦截器
instance.interceptors.request.use(
  function (config) {
    // 显示加载中
    loadingInstance = Loading.service({ text: '加载中...', fullscreen: true })

    // 携带 token
    const token = getToken()
    if (token) {
      config.headers.Authorization = `${token}`
    }

    return config
  },
  function (error) {
    if (loadingInstance) loadingInstance.close()
    console.log(error)
    // return Promise.reject(error)
  }
)

// 响应拦截器
instance.interceptors.response.use(
  function (response) {
    if (loadingInstance) loadingInstance.close()
    console.log('响应拦截器：', response)
    // 处理业务错误
    if (response.data.statusCode === 200) {
      Message({
        message: response.data.message || '请求成功',
        duration: 2000,
        showClose: true,
        type: 'success'
      })
    }
    if (response.data.statusCode !== 200) {
      Message({
        message: response.data.message || '请求失败',
        duration: 2000,
        showClose: true,
        type: 'error'
      })
    //   return Promise.reject(new Error(response.data.message || '请求失败'))
    }

    return response.data
  },
  function (error) {
    console.log('响应拦截器错误：', error)
    if (loadingInstance) loadingInstance.close()

    let errorMessage = '请求错误'
    if (error.response) {
      const { status, data } = error.response
      switch (status) {
        case 400:
          errorMessage = data.message || '请求参数错误'
          break
        case 401:
          errorMessage = '未授权，请重新登录'
          break
        case 403:
          errorMessage = '无访问权限'
          break
        case 404:
          errorMessage = '资源不存在'
          break
        case 500:
          errorMessage = '服务器错误，请稍后再试'
          break
        default:
          errorMessage = data.message || '未知错误'
      }
    } else if (error.message.includes('timeout')) {
      errorMessage = '请求超时，请检查网络'
    } else {
      errorMessage = error.message
    }

    Message.error({
      message: errorMessage,
      duration: 2000,
      showClose: true
    })

    // return Promise.reject(new Error(errorMessage))
  }
)

export default instance
