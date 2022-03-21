import axios from  '../axios';

const postLogout = async (props: { RefreshToken: string }): Promise<string> => {
   const replace = !!props ? props : {};
   return await axios.instance.post('auth/logout', replace);
}

export default postLogout;