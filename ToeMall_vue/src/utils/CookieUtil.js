import Cookies from 'js-cookie'

const TOKEN_KEY = 'token'

// **保存 Token**
export function saveToken (token) {
  Cookies.set(TOKEN_KEY, token, {
    expires: 7,
    // secure: true, // 仅在 HTTPS 下存储，提高安全性
    sameSite: 'Strict' // 防止 CSRF 攻击
  })
}

// **获取 Token**
export function getToken () {
  return Cookies.get(TOKEN_KEY) || null
}

// **删除 Token**
export function removeToken () {
  Cookies.remove(TOKEN_KEY)
}
