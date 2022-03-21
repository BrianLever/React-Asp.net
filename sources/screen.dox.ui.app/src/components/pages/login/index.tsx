import React, { useState, useEffect } from 'react';
import Card from '../../UI/card/Card';
import classes from './Login.module.scss';
import { useDispatch, useSelector } from 'react-redux';
import { setCurrentPage } from 'actions/settings';
import { ERouterKeys, ERouterUrls } from 'router';
import { FormControl, Select, TextField, Grid, Button, CircularProgress } from '@material-ui/core';
import { isLoginRequestLoadingSelector, loginErrorListSelector } from 'selectors/login';
import { loginRequest } from 'actions/login';
import { getToken } from 'helpers/auth';
import { useHistory, Redirect } from 'react-router';
import backgroundImage from '../../../assets/login_background.jpeg';
import config from 'config/app.json';



const Login = (props: any) => {console.log(props)
  const [enteredUsername, setenteredUsername] = useState('');
  const [emailIsValid, setEmailIsValid] = useState(true);
  const [enteredPassword, setEnteredPassword] = useState('');
  const [passwordIsValid, setPasswordIsValid] = useState(true);
  const [formIsValid, setFormIsValid] = useState(false);
  const [remember, setRemember] = useState(true);
  const isLoading: boolean = useSelector(isLoginRequestLoadingSelector);
  const errorList: Array<string> = useSelector(loginErrorListSelector);
  const dispatch = useDispatch();
  const history = useHistory();

  useEffect(() => {
    dispatch(setCurrentPage(ERouterKeys.LOG_IN, ERouterKeys.LOG_IN));
  }, [getToken('token')]);


  const emailChangeHandler = (event: React.ChangeEvent<HTMLInputElement>) => {
    const value = event.target.value;
    setenteredUsername(value);
    setFormIsValid(value !== "" && enteredPassword.trim().length > 6);
  };

  const passwordChangeHandler = (event: React.ChangeEvent<HTMLInputElement>) => {
    const value = event.target.value;
    setEnteredPassword(value);
    setFormIsValid(value !== "" && value.trim().length > 6);
  };

  const validateEmailHandler = () => {
    setEmailIsValid(enteredUsername !== "");
  };

  const validatePasswordHandler = () => {
    setPasswordIsValid(enteredPassword.trim().length > 6);
  };

  const submitHandler = (event: React.ChangeEvent<HTMLFormElement>) => {
    event.preventDefault();
    dispatch(loginRequest({Username: enteredUsername, Password: enteredPassword}))
  };


  return (
    <div  className={classes.login} >
        <div className={classes.extra_container}>

        </div>
        <div className={classes.form_container}>
            <div className={classes.form_container_login}>
               <h1><a href=''>Screendox logo</a></h1>
               <form className={classes.loginForm} onSubmit={submitHandler}>
                 {!isLoading && errorList && errorList.map((error, index) => (
                   <p className={classes.errors}>
                     {error}
                   </p>
                 ))}
                  <p>
                    <label>Username</label>
                    <input type="text"  autoCapitalize={'off'} value={enteredUsername} onChange={emailChangeHandler}>
                    </input>
                  </p>
                  <div className={classes.usr_pass_wrap}>
                      <label>Password</label>
                      <div style={{ position: 'relative' }}>
                        <input type="password" size={20}  value={enteredPassword} onChange={passwordChangeHandler}>
                        </input>
                        <button type="button" className={classes.password_button}>
                          <span className={classes.dashicons}>
                          </span>
                        </button>
                      </div>
                  </div>
                  <p className={classes.submit}>
                    <button className={classes.submitButton} disabled={isLoading} type="submit">
                      Log In
                    </button>
                  </p>
               </form>
               <div className={classes.resetPassword}>
                  <a href={config.BASEURL+ERouterUrls.RESET_PASSWORD} style={{ textDecoration: 'none', color: 'white' }}>Lost your password?</a>
               </div>
            </div>
        </div>
    </div>
  );
};

export default Login;
