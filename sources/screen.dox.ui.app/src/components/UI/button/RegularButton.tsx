import React from 'react';
import { IButtonProps } from './Button';
import Button from '@material-ui/core/Button';
import classes from './Button.module.scss';

export interface IRegularButton extends IButtonProps {}

const RegularButton = (props: IRegularButton): React.ReactElement => {
    return (
        <Button 
            size="small" 
            className={`${classes.regularButtonColor} ${props.disabled ? classes.regularButtonDisabledColor : ''}`}
            onClick={props.onClick}
            disabled={props.disabled}
        >
            { props.children }
        </Button>
    )

}

export default RegularButton;