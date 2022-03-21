import React, { useEffect, useState } from 'react';
import { Grid, TextField, Button, FormControl, Select } from '@material-ui/core';
import {  ChangePasswordLabelText } from '../../styledComponents';
import { useSelector, useDispatch  } from 'react-redux';
import classes from '../../pages.module.scss';
import { changeSecurityQuestionListSelector } from '../../../../selectors/security-question';
import { changeSecurityQuestionUpdateRequest } from '../../../../actions/change-security-question';
import {EMPTY_LIST_VALUE} from 'helpers/general'
import ScreendoxSelect from 'components/UI/select';



const ChangeSecurityQuestionPage = (): React.ReactElement => {
    const dispatch = useDispatch();
    const [currentPassword, setCurrentPassword] = useState('');
    const [question, setQuestion] = useState('');
    const [answer, setAnswer] = useState('');
    const [isLoading, setIsLoading] = useState(false);
    const questions = useSelector(changeSecurityQuestionListSelector)

    const currentPasswordHangleChange = (value: any) => {
        setCurrentPassword(value);
    }

    const questionHandleChange = (value: any) => {
        setQuestion(value);
    }

    const answerHandleChange = (value: any) => {
        setAnswer(value);
    }

    const handleSubmit = () => {
        dispatch(changeSecurityQuestionUpdateRequest({
            Password: currentPassword,
            SecurityQuestion: question,
            SecurityQuestionAnswer: answer
        }))
    }


    return (
        <div className={classes.centerContainer}>
            <Grid container>
                <Grid container>
                    <Grid item sm={6} style={{ textAlign: 'center' }}>
                        <ChangePasswordLabelText>
                            Current Password*
                        </ChangePasswordLabelText>
                    </Grid>
                    <Grid item xs={6} style={{ textAlign: 'left' }}>
                        <TextField
                            margin="dense"
                            label=""
                            error={!currentPassword && isLoading}
                            id="current_password"
                            type="password"
                            variant="outlined"
                            value={currentPassword}
                            onChange={e => {
                                e.stopPropagation();
                                currentPasswordHangleChange(e.target.value);
                            }}
                        />
                    </Grid>
                </Grid>
                <Grid container>
                    <Grid item sm={6} style={{ textAlign: 'center' }}>
                        <ChangePasswordLabelText>
                            Security Question*
                        </ChangePasswordLabelText>
                    </Grid>
                    <Grid item xs={6} style={{ textAlign: 'left' }}>
                        <ScreendoxSelect
                            options={questions.map((l) => (
                                { name: l, value: l }
                            ))}
                            defaultValue={question}
                            rootOption={{ name: EMPTY_LIST_VALUE, value: '0' }}
                            changeHandler={(value: any) => {
                                dispatch(questionHandleChange(value))
                            }}
                        />
                    </Grid>         
                </Grid>
                <Grid container>
                    <Grid item sm={6} style={{ textAlign: 'center' }}>
                        <ChangePasswordLabelText>
                            Security Answer*
                        </ChangePasswordLabelText>
                    </Grid>
                    <Grid item xs={6} style={{ textAlign: 'left' }}>
                        <TextField
                            margin="dense"
                            label=""
                            error={!answer && isLoading}
                            id="answer"
                            variant="outlined"
                            value={answer}
                            onChange={e => {
                                e.stopPropagation();
                                answerHandleChange(e.target.value);
                            }}
                        />
                    </Grid>         
                </Grid>
                
                <Grid item xs={12} sm={12} style={{ textAlign: 'center', marginTop: 20 }}>
                    <Grid container >
                        <Grid item sm={6} style={{ textAlign: 'right' }}>
                            <Button 
                            size="large"  
                            disabled={false}
                            variant="contained" 
                            color="primary" 
                            style={{ backgroundColor: '#2e2e42', textAlign: 'right' }}
                            onClick={() => {
                                handleSubmit();
                            }}
                            >
                                <p style={{ color: '#fff' }}>
                                    Save Changes
                                </p>
                            </Button>
                        </Grid>
                        <Grid item sm={6} >
                            <Button 
                            size="large" 
                            disabled={false}
                            variant="contained" 
                            color="default"
                            style={{ backgroundColor: 'transparent', border: '1px solid #2e2e42', textAlign: 'left' }}
                            onClick={() => {
                                
                            }}
                            >
                                <p style={{ color: '#2e2e42' }}>
                                    Cancel
                                </p>
                            </Button>
                        </Grid>
                    </Grid>
                </Grid>
            </Grid>
        </div>
    )
}

export default ChangeSecurityQuestionPage;