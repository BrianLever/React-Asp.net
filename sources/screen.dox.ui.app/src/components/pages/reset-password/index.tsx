
import React, { useState, useEffect } from 'react';
import { ERouterKeys, ERouterUrls } from 'router';
import Card from '../../UI/card/Card';
import classes from './ResetPassword.module.scss';
import config from 'config/app.json';
import { useDispatch, useSelector } from 'react-redux';
import { isResetPasswordLoadingErrorSelector, isResetPasswordLoadingSelector, isResetPasswordSecurityQuestionLoadingErrorSelector, isResetPasswordSecurityQuestionLoadingSelector, isResetPasswordSuccessSelector, resetPasswordErrorsSelector, resetPasswordSecurityQuestionSelector } from 'selectors/reset-password';
import { setCurrentPage } from 'actions/settings';
import { resetPasswordGetSecurityQuestionRequest, resetPasswordRequest } from 'actions/reset-password';

const ResetPassword = (props: any) => {
  const dispatch = useDispatch();
  const isSecurityQuestionLoadingError: boolean = useSelector(isResetPasswordSecurityQuestionLoadingErrorSelector)
  const isSecurityQuestionLoading: boolean = useSelector(isResetPasswordSecurityQuestionLoadingSelector);
  const securityQuestion: string | null = useSelector(resetPasswordSecurityQuestionSelector);
  const isResetPasswordLoading: boolean = useSelector(isResetPasswordLoadingSelector);
  const isResetPasswordLoadingError: boolean = useSelector(isResetPasswordLoadingErrorSelector);
  const resetPasswordErrors: Array<string> = useSelector(resetPasswordErrorsSelector);
  const isSuccess: boolean = useSelector(isResetPasswordSuccessSelector);
  const [username, setUsername] = useState('');
  const [error, setError] = useState('');
  const [answer, setAnswer] = useState('');
  const [password, setPassword] = useState('');

  console.log(isResetPasswordLoading, isResetPasswordLoadingError, resetPasswordErrors)

  useEffect(() => {
      dispatch(setCurrentPage(ERouterKeys.RESET_PASSWORD, ERouterUrls.RESET_PASSWORD));
      if(isSecurityQuestionLoadingError) {
          setError('You have entered invalid user name or your account has been blocked.')
      }
  }, [isSecurityQuestionLoadingError])

  const handleSubmit = () => {
    dispatch(resetPasswordRequest({
        SecurityQuestionAnswer: answer, 
        NewPassword: password, 
        username: username
    }))
  }

  return (
    <div  className={classes.resetpassword} >
        <div className={classes.extra_container}>

        </div>
        <div className={classes.form_container}>
            <div className={classes.form_container_resetpassword}>
               <h1><a href={config.BASEURL+ERouterUrls.DASHBOARD}>Screendox logo</a></h1>
               {securityQuestion && !isResetPasswordLoading && !isResetPasswordLoadingError && isSuccess?
               <>
                    <p className={classes.message}>Check your email for the confirmation link, then visit the <a href={config.BASEURL+ERouterUrls.LOGIN} style={{ color: '#2271b1'}}>login page.</a></p>
               </>:
               <>
                    {(!securityQuestion)?
                <>
                   <p className={classes.message}>
                        Please enter your username or email address. You will receive an email message with instructions on how to reset your password.
                   </p>
                    <form className={classes.resetpasswordForm}>
                        <p className={classes.errors}>
                            {error}
                        </p>
                        <p>
                            <label>Username</label>
                            <input type="text"  autoCapitalize={'off'} onChange={(e) => setUsername(e.target.value)}
                                onKeyDown={(e) => {
                                    if(e.keyCode == 13){
                                        e.preventDefault();
                                        if(username === '') {
                                            setError('Username is Required.');
                                            return;
                                        }
                                        dispatch(resetPasswordGetSecurityQuestionRequest(username))
                                     }
                                }}
                            >
                            </input>
                        </p>
                        <p className={classes.submit}>
                            <button className={classes.submitButton} type={'button'} disabled={isSecurityQuestionLoading} 
                            onClick={() => {
                                if(username === '') {
                                    setError('Username is Required.');
                                    return;
                                }
                                dispatch(resetPasswordGetSecurityQuestionRequest(username))
                            }}>
                            Continue
                            </button>
                        </p>
                    </form>
                    <div className={classes.resetPassword}>
                        <a href={config.BASEURL+ERouterUrls.LOGIN} style={{ textDecoration: 'none', color: 'white' }}>Login</a>
                    </div>     
                </>:
                <>
                    <form className={classes.resetpasswordForm} style={{ height: 430 }}>
                        {isResetPasswordLoadingError && resetPasswordErrors && resetPasswordErrors.map((resetPasswordError, index) => (
                             <p className={classes.errors} key={index}>
                               {resetPasswordError}
                             </p>
                        ))}
                        <label>Question: {securityQuestion}</label>
                        <p>
                            <label>Answer</label>
                            <input type="text"  autoCapitalize={'off'} onChange={(e) => setAnswer(e.target.value)}
                                onKeyDown={(e) => {
                                    if(e.keyCode == 13){
                                        e.preventDefault();
                                        handleSubmit()
                                     }
                                }}
                            >
                            </input>
                        </p>
                        <p>
                            <label>New Password</label>
                            <input type="password"  autoCapitalize={'off'} onChange={(e) => setPassword(e.target.value)}
                                onKeyDown={(e) => {
                                    
                                    if(e.keyCode == 13){
                                        e.preventDefault();
                                        handleSubmit()
                                     }
                                }}
                            >
                            </input>
                        </p>
                        <p className={classes.submit}>
                            <button className={classes.submitButton} type={'button'} 
                            disabled={isResetPasswordLoading} 
                            onClick={handleSubmit}>
                            Submit
                            </button>
                        </p>
                    </form>
                    <div className={classes.resetPassword}>
                        <a href={config.BASEURL+ERouterUrls.LOGIN} style={{ textDecoration: 'none', color: 'white' }}>Login</a>
                    </div>            
                </>
                }
               </>
               }
            </div>
        </div>
    </div>
  );
};

export default ResetPassword;
