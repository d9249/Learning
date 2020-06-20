import gym
import sys
import itertools
import numpy as np
import tensorflow as tf
import tensorflow.contrib.layers as layers

import common.tf_util as U
import logger
import deepq
from deepq.replay_buffer import ReplayBuffer
from deepq.utils import ObservationInput
from common.schedules import LinearSchedule


def model(inpt, num_actions, scope, reuse=False):
    """This model takes as input an observation and returns values of all actions."""
    #해당 모델은 관찰을 입력으로 하고 모든 동작의 값을 반환한다.
    with tf.variable_scope(scope, reuse=reuse):
        out = inpt
        #num_outputs 값 수정(32, 64, 128)
        #activation_function으로 tangent hyperbolic을 사용. (-1부터 1 사이의 값을 갖는다.)
        #Sigmoid보다 더 표현 가능한 범위가 넓고 기울기 특성도 좋아 bound가 필요하지만 표현력도 필요한 재귀형 신경망(recurrent neural network)에서 많이 보인다.
        out = layers.fully_connected(out, num_outputs=64, activation_fn=tf.nn.tanh)
        ################
        out = layers.fully_connected(out, num_outputs=num_actions, activation_fn=None)
        return out

if __name__ == '__main__':
    with U.make_session(num_cpu=1):
        env = gym.make("CartPole-v0") #학습을 하기 위해 환경 생성.
        #모델을 교육하는 데 필요한 모든 기능 생성.
        act, train, update_target, debug = deepq.build_train(
            make_obs_ph=lambda name: ObservationInput(env.observation_space, name=name),
            q_func=model, #model input
            num_actions=env.action_space.n,
            optimizer=tf.train.AdamOptimizer(learning_rate=5e-4), #최적화 알고리즘으로 Adam을 사용.
        )
        #replay buffer 생성
        #repaly_buffer.py에서 E-Greedy 를 따라 작은 확률로 랜덤하게 가고, 큰 확률로 높은 Q 를 따르는 쪽으로 간다.
        replay_buffer = ReplayBuffer(50000) #update 효율을 증가시키기 위해서 ReplayBuffer을 50000으로 설정.
        # Create the schedule for exploration starting from 1 (every action is random) down to
        # 0.02 (98% of actions are selected according to values predicted by the model).
        #반복 작업을 쉽게 모형화하며, 시간과 공간 데이터 모두 시각화를 도와주고, work의 연속성을 확인시켜주기 위해 LinearSchedule를 사용.
        exploration = LinearSchedule(schedule_timesteps=10000, initial_p=1.0, final_p=0.02)

        #매개 변수를 초기화하고 대상 네트워크에 복사.
        U.initialize()
        update_target()
        reward_list = []  #reward들을 파일에 저장하기 위한 list.
        episode_rewards = [0.0]
        obs = env.reset() # 환경을 초기화

        #총 보상과 에피소드별 단계를 포함하는 목록 작성
        for t in itertools.count():
            #action을 취하고, 최신의 exploration로 update
            action = act(obs[None], update_eps=exploration.value(t))[0] #에이전트의 움직임.
            new_obs, rew, done, _ = env.step(action) #움직임에 따른 결과값들, 환경으로부터 새로운 상태 및 보상 받기
            #replay buffer에 transition을 저장.
            replay_buffer.add(obs, action, rew, new_obs, float(done))
            reward_list.append(rew) #리스트에 reward를 추가.
            obs = new_obs

            episode_rewards[-1] += rew
            if done:
                obs = env.reset() #완료 되었다면, 다시 반복하기 위해 환경 초기화
                episode_rewards.append(0)

                #Reward 파일에 저장(파일명 변경)
                with open("../../32neurons_1.txt", "a") as f:
                    f.write(str(sum(reward_list)) + "\n")
                reward_list = []  #reward list 초기화

            #종료 조건
            is_solved = t > 100 and np.mean(episode_rewards[-101:-1]) >= 200
            is_finished = len(episode_rewards) > 2000 and is_solved

            if is_solved:
                if len(episode_rewards) > 1998:
                    # Show off the result
                    env.render() #환경을 화면으로 출력
                if is_finished:
                    sys.exit(0)

            else:
                #재생 버퍼에서 샘플링된 배치에서 Bellman 방정식의 오류를 최소화한다.
                #최적의 가치함수를 찾기 위해서는 단순히 현재 에이전트의 정책에 대한 가치함수를 구하는 것이 아니라 현재의 정책을 최적의 정책으로 업데이트 하기위해 적용.
                if t > 1000:
                    #Replay Buffer Sample 수 수정(16, 32, 64)
                    obses_t, actions, rewards, obses_tp1, dones = replay_buffer.sample(32)
                    ################
                    train(obses_t, actions, rewards, obses_tp1, dones, np.ones_like(rewards))

                # Update target network periodically.
                # Target Update 주기 수정(250, 500, 1000)
                if t % 1000 == 0:
                    #################
                    update_target()

            #학습 진행도를 확인하기 위한 부분.
            if done and len(episode_rewards) % 10 == 0:
                logger.record_tabular("steps", t)
                logger.record_tabular("episodes", len(episode_rewards))
                logger.record_tabular("mean episode reward", round(np.mean(episode_rewards[-101:-1]), 1))
                logger.record_tabular("% time spent exploring", int(100 * exploration.value(t)))
                logger.dump_tabular()

