import axios, { AxiosInstance } from 'axios';
import { getToken, setToken } from 'helpers/auth';
import jwt_decode from "jwt-decode";
import { ERouterUrls } from 'router';
import * as appConfig from '../config/app.json';
const BASE_URL = appConfig.REACT_APP_BASE_URL;



const axiosObject: AxiosInstance = axios.create({
    headers: {
        "Content-Type": "application/json"
    },
    baseURL: BASE_URL,
});


axiosObject.interceptors.request.use(
    async config => {
      if(typeof(config.data) == "undefined") {
          config.data = {}
      }

      const token = getToken('token');
      
      config.headers = { 
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      }

      if(token) {
        config.headers = {
            ...config.headers,
            'Authorization': 'Bearer ' + token,
        }
      }
      return config;
    },
    error => {
      Promise.reject(error)
});
  

export interface IPdfFileDownload {
    Filename: string;
    Data: Blob
}

export interface IExcelFileDownload {
    Filename: string;
    Data: Blob
}

axiosObject.interceptors.response.use((response) => {

    if(response.headers['content-type'] === 'application/pdf' || response.headers['content-type'] === 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet')
    {
        const data: IPdfFileDownload = {
            Filename: getFileName(response), 
            Data: response?.data || {}
        };
        
        return Promise.resolve(data)
    }

    if(response.headers['content-type'] === 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet')
    {
        const data: IExcelFileDownload = {
            Filename: getFileName(response), 
            Data: response?.data || {}
        };
        
        return Promise.resolve(data)
    }
    
    const { data = {} } = response || {}

    return Promise.resolve(data);
},
    async (err) => {
        const originalConfig = err.config;
        if (originalConfig.url === "auth/refreshtoken") {
            // Access Token was expired
            try {
                if (err.response.status === 400 && !originalConfig._retry) { 
                    localStorage.clear();
                    window.location.href = appConfig.BASEURL+ERouterUrls.LOGIN;
                } else {
                    setTimeout(() => {
                        originalConfig._retry = true;console.log('[Auth] Refresh token error. Retry')
                    }, 3000)
                }
             }
             catch(e) {
                setTimeout(() => {
                    originalConfig._retry = true;console.log('[Auth] Refresh token error.Retry.')
                }, 3000)
             }
        }

        return Promise.reject(err);
    }
);


// get file name from Content-Disposition header
function getFileName (response: any) {

    response = response || {};

    var headers = response.headers;

    if(headers){
        // Trying to get file name from response header
        var contentDisposition = response.headers['content-disposition'] || "";
        const re = /filename="(.+)"/;

        let filename = contentDisposition.match(re)[1];

        return filename;
    }
    
    return null;
}

class Axios {
    public instance: AxiosInstance;
    constructor(axios: AxiosInstance) {
        this.instance = axios;
    }
}



export default new Axios(axiosObject);

