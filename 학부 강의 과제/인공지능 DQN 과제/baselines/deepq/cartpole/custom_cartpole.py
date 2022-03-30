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
import time

#학습소요시간을 측정하기 위한 time method 사용
start = time.time()

def model(inpt, num_actions, scope, reuse=False):
    #해당 모델은 관찰을 입력으로 하고 모든 동작의 값을 반환한다.
    with tf.variable_scope(scope, reuse=reuse):
        out = inpt
        #num_outputs 값 수정(32, 64, 128)
        #activation_function으로 tangent hyperbolic을 사용. (-1부터 1 사이의 값을 갖는다.)
        out = layers.fully_connected(out, num_outputs=64, activation_fn=tf.nn.tanh)
        ################
        out = layers.fully_connected(out, num_outputs=num_actions, activation_fn=None)
        return out

if __name__ == '__main__':
    with U.make_session(num_cpu=1):
        env = gym.make("CartPole-v0") #학습을 하기 위해 환경 생성.
        #모델을 교육하는 데 필요한 모든 기능 생성.
        # act = state -> action
        # train = 모델 학습
        # update_target = target network update
        act, train, update_target, debug = deepq.build_train(
            make_obs_ph=lambda name: ObservationInput(env.observation_space, name=name),
            q_func=model, #model input
            num_actions=env.action_space.n,
            optimizer=tf.train.AdamOptimizer(learning_rate=5e-4), #최적화 알고리즘으로 Adam을 사용.
        )
        #replay buffer 생성 부분
        replay_buffer = ReplayBuffer(50000) #update 효율을 증가시키기 위해서 ReplayBuffer을 50000으로 설정.
        # Create the schedule for exploration starting from 1 (every action is random) down to
        # 0.02 (98% of actions are selected according to values predicted by the model).
        #탐험을 하기 위해 98%의 확률로 모델에 의한 경로로 움직인다. 그리고 2% 확률로 랜덤한 행동을 취함.
        exploration = LinearSchedule(schedule_timesteps=10000, initial_p=1.0, final_p=0.02)

        #매개 변수를 초기화하고 대상 네트워크에 복사.
        U.initialize()
        update_target()
        reward_list = [] #reward들을 파일에 저장하기 위한 list.
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
            obs = new_obs #Step()함수의 새로운 결과 값을 obs에 저장.

            episode_rewards[-1] += rew #현재 episode의 reward에 나온 reward 값을 합산.
            if done:
                obs = env.reset() #완료 되었다면, 다시 반복하기 위해 환경 초기화
                episode_rewards.append(0) #episode_rewards에 다음 episode에서 학습할 리스트를 추가

                #Reward 파일에 저장(파일명 변경)
                with open("../../32neurons_1.txt", "a") as f:
                    f.write(str(sum(reward_list)) + "\n")
                reward_list = []  #reward list 초기화

            #종료
            #episode_rewards 리스트에 저장된 최근 100개의 평균이 200이 되면 문제 해결.
            is_solved = t > 100 and np.mean(episode_rewards[-101:-1]) >= 200
            #episode가 2000번을 넘게 된다면 문제 해결.
            is_finished = len(episode_rewards) > 2000 and is_solved

            if is_solved:
                if len(episode_rewards) > 1998:
                    # Show off the result
                    env.render() #환경을 화면으로 출력
                if is_finished:
                    sys.exit(0)
            else:
                #재생 버퍼에서 샘플링된 배치에서 Bellman 방정식의 오류를 최소화한다.
                if t > 1000:
                    #replay memory에서 (16, 32, 64)batch size만큼의 기록을 랜덤 추출.
                    #Replay Buffer Sample 수 수정(16, 32, 64)
                    obses_t, actions, rewards, obses_tp1, dones = replay_buffer.sample(32)
                    ################
                    #추출한 기록을 학습.
                    train(obses_t, actions, rewards, obses_tp1, dones, np.ones_like(rewards))

                # Update target network periodically.
                # Target Update 주기 수정(250, 500, 1000)
                if t % 1000 == 0:
                    #################
                    update_target()

            #학습 진행도를 확인하기 위한 부분. 에피소드가 완료 상태일 때 10번 단위로 결과를 출력
            if done and len(episode_rewards) % 10 == 0:
                logger.record_tabular("steps", t)
                logger.record_tabular("episodes", len(episode_rewards))
                logger.record_tabular("mean episode reward", round(np.mean(episode_rewards[-101:-1]), 1))
                logger.record_tabular("% time spent exploring", int(100 * exploration.value(t)))
                print(time.time() - start)
                logger.dump_tabular()

