import React, { useEffect, useState, ChangeEvent } from 'react';
import { Grid, TextField, Button, FormControl, Select } from '@material-ui/core';
import RectangleCheckbox from 'components/UI/checkbox/RectangleCheckbox';
import { useDispatch, useSelector } from 'react-redux';
import { ProfileTitle, ProfileLargeBoldText, ProfileLargeText, ProfileSmallText, TextArea, useStyles } from '../styledComponent';
import { changeSecurityQuestionListSelector } from 'selectors/security-question';
import { changeSecurityQuestionUpdateRequest } from 'actions/change-security-question';
import {EMPTY_LIST_VALUE} from 'helpers/general'
import ScreendoxSelect from 'components/UI/select';

const SecurityQuestion = (): React.ReactElement => {
    const dispatch = useDispatch();
    const [currentPassword, setCurrentPassword] = useState('');
    const [question, setQuestion] = useState('');
    const [answer, setAnswer] = useState('');
    const [isLoading, setIsLoading] = useState(false);
    const questions = useSelector(changeSecurityQuestionListSelector)
    const classes = useStyles();
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

    return (<>
        <Grid container justifyContent="space-between" alignItems="center" style={{ marginTop: 40 }} spacing={2}>  
           <Grid item sm={12}>
                <ProfileLargeBoldText>Password</ProfileLargeBoldText>
                <TextField
                        fullWidth
                        margin="dense"
                        id="Password"
                        error={!currentPassword && isLoading}
                        type={'password'}
                        variant="outlined"
                        value={currentPassword}
                        onChange={e => {
                            e.stopPropagation();
                            currentPasswordHangleChange(e.target.value);
                        }}

                />
           </Grid>
        </Grid>
        <Grid container justifyContent="space-between" alignItems="center" style={{ marginTop: 20 }} spacing={2}>  
           <Grid item sm={12}>
                <ProfileLargeBoldText>Security Question*</ProfileLargeBoldText>
                <ScreendoxSelect
                    options={questions.map((l) => (
                        { name: l, value: l }
                    ))}
                    defaultValue={question}
                    rootOption={{ name: EMPTY_LIST_VALUE, value: '' }}
                    changeHandler={(value: any) => {
                        questionHandleChange(value);
                    }}
                />
           </Grid>
        </Grid>
        <Grid container justifyContent="space-between" alignItems="center" style={{ marginTop: 20 }} spacing={2}>  
           <Grid item sm={12}>
                <ProfileLargeBoldText>Security Answer*</ProfileLargeBoldText>
                <TextField
                    fullWidth
                    margin="dense"
                    id="Security Answer"
                    variant="outlined"
                    value={answer}
                    onChange={e => {console.log(e.target.value);
                        e.stopPropagation();
                        answerHandleChange(e.target.value);
                    }}
                />
           </Grid>
        </Grid>                
        <Grid container>
            <Grid item sm={12} style={{ textAlign: "right", marginTop: 40 }}>
                <Button 
                    size="large" 
                    disabled={isLoading}
                    variant="contained" 
                    color="primary" 
                    style={{ backgroundColor: '#2e2e42' }}
                    className={classes.buttonStyle}
                    onClick={() => {
                        handleSubmit();
                    }}
                >
                    <p className={classes.buttonTextStyle}>
                        Save Changes
                    </p>
                </Button>
            </Grid>
            <Grid item sm={12} style={{ textAlign: "right", marginTop: 10}}>
                <Button 
                    size="large" 
                    disabled={false}
                    variant="contained" 
                    color="primary" 
                    className={classes.buttonStyle}
                    style={{ backgroundColor: '#2e2e42' }}
                    onClick={() => {

                    }}
                >
                    <p className={classes.buttonTextStyle}>
                        Cancel
                    </p>
                </Button>
            </Grid>
        </Grid></>
    )
}

export default SecurityQuestion;