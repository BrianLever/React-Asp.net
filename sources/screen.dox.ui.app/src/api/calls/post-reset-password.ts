import axios from  '../axios';

const postResetPassword = async (username: string, props: { 
    SecurityQuestionAnswer: string,
    NewPassword: string;
}): Promise<string> => {
    const replace = !!props ? props : {};
    return await axios.instance.post(`auth/resetpassword/`+username,{       
        ...replace 
    });
}

export default postResetPassword;

