import React from 'react';
import { TComponentChildrenType } from '../../../common-types/component.type';
import RegularButton from './RegularButton';

export interface IButtonProps {
  className?: string;
  disabled?: boolean; 
  type: 'submit' | 'button' | 'regular';
  color: 'regular' | 'regular-disbaled';
  children?: TComponentChildrenType;
  onClick?: () => void;
}

const Button = (props: IButtonProps): React.ReactElement => {
  
  switch(props.type) {
    case 'regular':
      return (<RegularButton {...props} />);
    default: return (
      <button
        { ...props }
        type={'button'}
        onClick={e => {
          e.stopPropagation();
          props.onClick && props.onClick()
        }}
      >
        {props.children}
      </button>
    );
  }
};

export default Button;
